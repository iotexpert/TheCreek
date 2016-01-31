/*******************************************************************************
* File Name:  B_SPI_Master_v2_0.v 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description: 
*  This file provides a base level model of the SPI Master component
*
* Note: 
*  None
********************************************************************************
*                 Control and Status Register definitions
******************************************************************************** 
*
*  Control Register Definition
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  | Desc  | unused | unused | unused | unused |unused  |unused  |unused  | TX_EN  |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*
*      tx_en        =>   0 = disable tx (for Bidirectional Mode only)
*                        1 = enable  tx (fof Bidirectional Mode only)
*      
*  Tx interrupt Status Register Definition
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  | Desc |interrupt|unused  |unused  |spi_idle|bt_cpmlt|tx_f_n_f|tx_f_emp|spi_done|
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+

*  Rx interrupt Status Register Definition
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  | Desc |interrupt|rx_overr|rx_n_emp|rx_full |unused  | unused |unused  | unused |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*
*    spi_done   =>  0 = spi transmission not done
*                   1 = spi transmission done
*
*    tx_f_e     =>  0 = TX FIFO not empty
*                   1 = TX FIFO empty
*
*    tx_f_n_f   =>  0 = TX_FIFO full
*                   1 = TX FIFO not full
*
*    rx_f_fll   =>  0 =  RX FIFO not full
*                   1 =  RX FIFO full
*
*    rx_f_n_e   =>  0 = RX FIFO empty
*                   1 = RX FIFO not empty
*
*    rx_f_over  =>  0 = RX FIFO not overrun
*                   1 = RX FIFO overrun
*    
*    bt_cmplt   =>  0 = byte transfer is not complete 
*                   1 = byte transfer complete
*
******************************************************************************** 
*                 Data Path register definitions
******************************************************************************** 
*  INSTANCE NAME:  DatapathName 
*  DESCRIPTION:
*  REGISTER USAGE:
*    F0 => TX FIFO buffer
*    F1 => RX FIFO buffer
*    D0 => na
*    D1 => na
*    A0 => SPI Master TX value  
*    A1 => SPI Master RX value
*
******************************************************************************** 
*               I*O Signals:   
******************************************************************************** 
*  IO SIGNALS: 
*
*	 reset        input     component reset input
*    clock        input     component clock input 
*    miso         input     SPI MISO input 
*    sclk         output    SPI SCLK output
*    ss           output    SPI SS output
*    tx_enable    output    tx enable output(is used for Bidirectional Mode only)
*    mosi         output    SPI MOSI output
*    interrupt    output    interrupt output
*
********************************************************************************
/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/
`include "cypress.v"
`ifdef B_SPI_Master_v2_0_V_ALREADY_INCLUDED
`else
`define B_SPI_Master_v2_0_V_ALREADY_INCLUDED
module B_SPI_Master_v2_0(
    input	wire reset,      /* System Reset                               */
    input	wire clock,      /* User Supplied clock = 2x bitrate of output */
    input	wire miso,       /* SPI MISO input                             */
    
    output	wire mosi,       /* SPI MOSI output              */
    output	reg  sclk,       /* SPI SCLK output              */
    output	reg  ss,         /* SPI SS output                */
	output  wire tx_interpt, /* Interrupt output             */
    output  wire rx_interpt, /* Interrupt output             */
    output  wire tx_enable   /* TX enable (for BidirectMode) */
);
    
    /* Status Register inputs from the PLD/DP's */
    wire [6:0] tx_status;             
    wire [6:0] rx_status;
    /*  Master Out Slave In from the Datapath. Selects between mosi_dp8 and mosi_dp16 based on NUM_BITS */
    wire mosi_from_dp;       
    
    /* One compare output of the counter which signals when to load received data into the FIFO */
    wire dpcounter_one;      
    wire dpcounter_zero;
       
    /* 7-bit counter output used for a compare to one output */
    wire [6:0] count;              
    
    wire miso_to_dp;
    
    /* MOSI FIFO Status outputs */
    wire dpMOSI_fifo_not_empty;
	wire nc1,nc2,nc3,nc4;
    reg [2:0] state;  
  
    /* bit order: default is MSb first (i.e Shift Left and ShiftLeft in static configuration is = 0) */
    /* DO NOT CHANGE these two parameters.  They define constants */
    localparam SPIM_MSB_FIRST = 1'b0;
    localparam SPIM_LSB_FIRST = 1'b1;
    
	/* State Machine state names*/
	localparam SPIM_STATE_IDLE              = 3'h0;
	localparam SPIM_STATE_LOAD_TX_DATA      = 3'h1;
	localparam SPIM_STATE_SEND_TX_DATA      = 3'h2;
	localparam SPIM_STATE_CAPT_RX_DATA      = 3'h3;
	localparam SPIM_STATE_SHFT_N_LD_TX_DATA = 3'h4;
	localparam SPIM_STATE_SPI_DONE          = 3'h5;
	localparam SPIM_STATE_SEND_TX_DATA_2    = 3'h7;
	
	/* Status Register bits */
	localparam SPIM_STS_SPI_DONE_BIT          = 3'd0;
    localparam SPIM_STS_TX_FIFO_EMPTY_BIT     = 3'd1;
    localparam SPIM_STS_TX_FIFO_NOT_FULL_BIT  = 3'd2;
    localparam SPIM_STS_BYTE_COMPLETE_BIT     = 3'd3;
    localparam SPIM_STS_SPI_IDLE_BIT          = 3'd4;    
    localparam SPIM_STS_RX_FIFO_FULL_BIT      = 3'd4;
    localparam SPIM_STS_RX_FIFO_NOT_EMPTY_BIT = 3'd5;
    localparam SPIM_STS_RX_FIFO_OVERRUN_BIT   = 3'd6;
		
	localparam CTRL_TX_PERMISSION  = 1'b0;
    
    /*******************************************************************************
    *User parameters used to define how the component is compiled
    ******************************************************************************/
    parameter [0:0] ShiftDir         = SPIM_MSB_FIRST;
    parameter [6:0] NumberOfDataBits = 7'd8; /* set to 2-16 bits only. Default is 8 bits */
    parameter       BidirectMode     = 1'b1;
  
    parameter [0:0] ModeCPHA = 1'b0; /* Default is rising edge mode */
    parameter [0:0] ModePOL  = 1'b0; /* Default is rising edge mode */
       
    localparam [2:0] dpMsbVal   = NumberOfDataBits[2:0]-3'b1;
	localparam [7:0] dpMISOMask = (NumberOfDataBits == 8 || NumberOfDataBits == 16) ? 8'b1111_1111 :
                                  (NumberOfDataBits == 7 || NumberOfDataBits == 15) ? 8'b0111_1111 :
                                  (NumberOfDataBits == 6 || NumberOfDataBits == 14) ? 8'b0011_1111 :
                                  (NumberOfDataBits == 5 || NumberOfDataBits == 13) ? 8'b0001_1111 :
                                  (NumberOfDataBits == 4 || NumberOfDataBits == 12) ? 8'b0000_1111 :
                                  (NumberOfDataBits == 3 || NumberOfDataBits == 11) ? 8'b0000_0111 :
                                  (NumberOfDataBits == 2 || NumberOfDataBits == 10) ? 8'b0000_0011 :
                                  (NumberOfDataBits == 9) ? 8'b0000_0001 : 8'b1111_1111;
    
	localparam [1:0] dynShiftDir     = (ShiftDir == SPIM_MSB_FIRST) ? 2'd1 : 2'd2;
    localparam [1:0] dp16MSBSIChoice = (ShiftDir == SPIM_MSB_FIRST) ? `SC_SI_A_CHAIN : `SC_SI_A_ROUTE;
    localparam [1:0] dp16LSBSIChoice = (ShiftDir == SPIM_MSB_FIRST) ? `SC_SI_A_ROUTE : `SC_SI_A_CHAIN;
    
	localparam f1_ld_src = (ModeCPHA == 1) ? `SC_FIFO1_ALU :`SC_FIFO1__A1;
	localparam [6:0] BitCntPeriod = (NumberOfDataBits << 1) - 1;
	localparam SR8  = 8'd8;
	
	localparam dp8_cfg = {
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
            dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG4 Comment:LDSHIFT */
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG5 Comment:END */    	
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            dpMISOMask, 8'h00,	/*CS_REG7 Comment: */
            8'hFF, 8'hFF,	/*SC_REG4	Comment: */
            `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_ROUTE, /*SC_REG5	Comment: */
            `SC_A0_SRC_ACC, ShiftDir, 1'h0,
            1'h0, f1_ld_src, `SC_FIFO0_BUS,
            `SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*SC_REG6 Comment: */
            3'h00, `SC_FIFO_SYNC__ADD, 6'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL 
        };
		
		localparam dp16_msb_cfg = {
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
        dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG4 Comment:LDSHIFT */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG5 Comment:END */    	
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, 
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, 
    	8'hFF, 8'h00,	
    	8'hFF, 8'hFF,	
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	dp16LSBSIChoice, /*SC_REG4	Comment: */
    	`SC_A0_SRC_ACC, ShiftDir, 1'h0,
    	1'h0, f1_ld_src, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG5	Comment: */
    	 3'h00, `SC_FIFO_SYNC__ADD, 6'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG6 Comment: */
    };

    localparam dp16_lsb_cfg = {
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
        dynShiftDir, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG4 Comment:LDSHIFT */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        dynShiftDir, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG5 Comment:END */    	
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, 
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        dynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, 
    	dpMISOMask, 8'h00,	
    	8'hFF, 8'hFF,	
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	dp16MSBSIChoice, /*SC_REG4	Comment: */
    	`SC_A0_SRC_ACC, ShiftDir, 1'h0,
    	1'h0, f1_ld_src, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG5	Comment: */
    	 3'h00, `SC_FIFO_SYNC__ADD, 6'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG6 Comment: */
    };
	
/*************************************************************************** 
*            Device Family and Silicon Revision definitions   
***************************************************************************/    
    /* PSoC3 ES2 or earlier */ 
    localparam PSOC3_ES2  = (`CYDEV_CHIP_MEMBER_USED   == `CYDEV_CHIP_MEMBER_3A &&  
                             `CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_3A_ES2); 
    /* PSoC5 ES1 or earlier */                         
    localparam PSOC5_ES1  = (`CYDEV_CHIP_MEMBER_USED   == `CYDEV_CHIP_MEMBER_5A &&  
                             `CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_5A_ES1); 
    
    /* Clock Enable primitive instantiation */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    ClkEn (
        .clock_in(clock),
        .enable(1'b1),
        .clock_out(clk_fin)
    );    
    
	wire dpMOSI_fifo_not_full;
    wire dpMOSI_fifo_empty;
    wire dpMISO_fifo_not_empty;
    wire dpMISO_fifo_full;
    wire miso_buf_overrun;
    wire tmp1;
    wire mosi_from_dpL;
    wire mosi_from_dpR;
    wire [7:0] control;
	wire cnt_tc;
	wire load_rx_data = dpcounter_one;
	wire pol_supprt = (ModePOL == 1) ? 1'b1: 1'b0;
	reg cnt_enable;
	reg byte_complete;
	reg mosi_reg;
	reg ld_ident;
 	assign mosi           = mosi_reg;	
	assign dpcounter_one  = (count == 5'h01);
	assign dpcounter_zero = (count == 5'h00);
	
    assign tx_status[SPIM_STS_SPI_DONE_BIT]          = (state == SPIM_STATE_SPI_DONE);
	assign tx_status[SPIM_STS_TX_FIFO_EMPTY_BIT]     = dpMOSI_fifo_empty;
	assign tx_status[SPIM_STS_TX_FIFO_NOT_FULL_BIT]  = dpMOSI_fifo_not_full;
	assign tx_status[SPIM_STS_BYTE_COMPLETE_BIT]     = dpcounter_one;
    assign tx_status[SPIM_STS_SPI_IDLE_BIT]          = (state == SPIM_STATE_IDLE);
    
    assign rx_status[SPIM_STS_RX_FIFO_FULL_BIT]      = dpMISO_fifo_full;
	assign rx_status[SPIM_STS_RX_FIFO_NOT_EMPTY_BIT] = dpMISO_fifo_not_empty;
	assign rx_status[SPIM_STS_RX_FIFO_OVERRUN_BIT]   = dpcounter_zero & dpMISO_fifo_full;
	
	assign tx_status[6:5] = 2'h0;
    assign rx_status[3:0] = 4'h0;
    
	generate
        if(BidirectMode) begin: BidirMode
            if(PSOC3_ES2 || PSOC5_ES1)
                begin: AsyncCtl
                    cy_psoc3_control #(.cy_force_order(1))
                   CtrlReg
                    (
                        /* output [07:00] */  .control(control)
                    );
                end /* AsyncCtl */
                else
                begin: SyncCtl
                    cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'h01))
                    CtrlReg
                    (
                        /*  input         */ .clock(clk_fin),
                        /* output [07:00] */ .control(control)
                    );
                end /* SyncCtl */
                
            assign tx_enable  = control[CTRL_TX_PERMISSION];
            assign miso_to_dp = (tx_enable == 1'b1) ? mosi : miso;
            
        end /* BidirectMode */
        else begin
            assign control    = 8'h00;
            assign tx_enable  = 1'b0;
            assign miso_to_dp = miso;
        end /*! BidirectMode*/
    endgenerate
	
	generate
	
	if(ModeCPHA == 1) 
	begin
	/* "CPHA == 1" State Machine implementation */
	    /* State Logic */
		always@(posedge clk_fin) begin
	        case(state) 
		        SPIM_STATE_IDLE: begin
			        if(!reset)begin
                        if(dpMOSI_fifo_empty)
                        begin
                            state <= SPIM_STATE_IDLE;
                        end
                        else begin
                            state <= SPIM_STATE_LOAD_TX_DATA;
                        end
                    end
                    else begin
                        state <= SPIM_STATE_IDLE;
                    end
			    end
			
			    SPIM_STATE_LOAD_TX_DATA: begin
                    if(!reset) begin
				        state <= SPIM_STATE_SEND_TX_DATA;
			        end
                    else begin
                        state <= SPIM_STATE_IDLE;
                    end
                end
			        
                SPIM_STATE_CAPT_RX_DATA: begin                    
                    if(!reset) begin
                        if(count[4:0] != 5'h01) 
                        begin
                            state <= SPIM_STATE_SEND_TX_DATA;	
                        end else 
                        begin
                            state <= SPIM_STATE_SPI_DONE; 
                        end
                    end
                    else begin
                        state <= SPIM_STATE_IDLE;
                    end
                end
			
                SPIM_STATE_SEND_TX_DATA: begin                    
                    if(!reset) begin    
                        if(count[4:0] != 5'h02) 
                        begin
                            state <= SPIM_STATE_CAPT_RX_DATA;
                        end else
                        begin
                            if(!dpMOSI_fifo_empty)
                            begin
                                state <= SPIM_STATE_SHFT_N_LD_TX_DATA;
                            end else 
                            begin
                                state <= SPIM_STATE_CAPT_RX_DATA;
                            end
                        end
                    end 
                    else 
                    begin
                        state <= SPIM_STATE_IDLE;
                    end
			    end
			
			    SPIM_STATE_SHFT_N_LD_TX_DATA: begin
                    if(!reset) begin
					    state <= SPIM_STATE_SEND_TX_DATA;
	                end
                    else begin
                        state <= SPIM_STATE_IDLE;
                    end
                end
						
			    SPIM_STATE_SPI_DONE: begin
			        state <= SPIM_STATE_IDLE;
	            end
			
		    default: begin
		        state <= SPIM_STATE_IDLE;
            end		
		    endcase
		
	    end /* END of CPHA ==1 State Machine implementation */
	
		/* Output Logic */
		always@(posedge clk_fin) begin 
		    case(state)
			    SPIM_STATE_IDLE: begin
			        ss         <= 1'b1;
                    cnt_enable <= 1'b0;
					mosi_reg   <= 1'b0;
					sclk       <= pol_supprt;
				end
								
				SPIM_STATE_LOAD_TX_DATA: begin
				    cnt_enable <= 1'b1;
				    ss         <= 1'b0;
				    mosi_reg   <= mosi_from_dp;
					sclk       <= pol_supprt;
			    end
				
			    SPIM_STATE_CAPT_RX_DATA: begin                    
                    if(count[4:0]!= 5'h01) 
				    begin
					    sclk  <= pol_supprt;
					end else 
				    begin
					   cnt_enable <= 1'b0;
					   sclk       <= pol_supprt;
					end
                end
				
				SPIM_STATE_SEND_TX_DATA: begin                    
                	sclk     <= ~pol_supprt;
				    mosi_reg <= mosi_from_dp;
				
			    end
				
				SPIM_STATE_SHFT_N_LD_TX_DATA: begin
					sclk  <= pol_supprt;
			    end
						
			    SPIM_STATE_SPI_DONE: begin
			        mosi_reg   <= 1'b0;
				    cnt_enable <= 1'b0;
					sclk       <= pol_supprt;
	            end
				default:
				begin
				    ss         <= 1'b1;
                    cnt_enable <= 1'b0;
					mosi_reg   <= 1'b0;
					sclk       <= pol_supprt;
				end
				endcase
		end
	
	
	/* "CPHA == 0" State Machine implementation */
	end else
	begin
	    /* State Logic */
		always@(posedge clk_fin) begin
		    case(state) 
		        SPIM_STATE_IDLE: begin
                    if(!reset) begin    
                        if(dpMOSI_fifo_empty)
                        begin
                            state <= SPIM_STATE_IDLE;
                        end
                        else begin
                            state <= SPIM_STATE_LOAD_TX_DATA;
                        end
                    end
                    else 
                    begin
                        state <= SPIM_STATE_IDLE;
                    end
			    end
			
			    SPIM_STATE_LOAD_TX_DATA: begin
                    if(!reset) begin
				        state <= SPIM_STATE_SEND_TX_DATA_2;
                    end
                    else 
                    begin
                        state <= SPIM_STATE_IDLE;
                    end
			    end
			    
				SPIM_STATE_SEND_TX_DATA_2: begin
                    if(!reset) begin
				        state <= SPIM_STATE_CAPT_RX_DATA;
                    end
                    else 
                    begin
                        state <= SPIM_STATE_IDLE;
                    end
				end				
                
				SPIM_STATE_CAPT_RX_DATA: begin                    
                    if(!reset) begin
                        if(count[4:0] != 5'h05) 
                        begin
                            state <= SPIM_STATE_SEND_TX_DATA;	
                        end else 
                        if(!dpMOSI_fifo_empty)begin
                            state <= SPIM_STATE_SHFT_N_LD_TX_DATA; 
                        end
                        else begin
                            state <= SPIM_STATE_SEND_TX_DATA;
                        end
                    end 
                    else 
                    begin
                        state <= SPIM_STATE_IDLE;
                    end
                end
			
                SPIM_STATE_SEND_TX_DATA: begin
                    if(!reset) begin                      		    					
                        if(count[4:0] != 5'h02)					
                        begin
                            state <= SPIM_STATE_CAPT_RX_DATA;
                        end 
                        else begin
                            if(!ld_ident) begin
                                state <= SPIM_STATE_SPI_DONE;
                            end else
                            begin
                                state <= SPIM_STATE_CAPT_RX_DATA;
                            end							
                        end
                    end 
                    else
                    begin
                        state <= SPIM_STATE_IDLE;
                    end
			    end
			
			    SPIM_STATE_SHFT_N_LD_TX_DATA: begin
                    if(!reset) begin
					    state <= SPIM_STATE_CAPT_RX_DATA;
			        end
                    else 
                    begin
                        state <= SPIM_STATE_IDLE;
                    end
                end
						
			    SPIM_STATE_SPI_DONE: begin
			        state <= SPIM_STATE_IDLE;
	            end
			
		    default: begin
		        state  <= SPIM_STATE_IDLE;
            end		
		    endcase
		end
		
		/* Output Logic */
		always@(posedge clk_fin) begin
		    case(state)
			SPIM_STATE_IDLE: begin
			    ss         <= 1'b1;
                cnt_enable <= 1'b0;
				mosi_reg   <= 1'b0;
				sclk       <= pol_supprt;
			end
			
			SPIM_STATE_LOAD_TX_DATA: begin
				ss       <= 1'b0;
				mosi_reg <= mosi_from_dp;
				sclk     <= pol_supprt;
			end
			
			SPIM_STATE_SEND_TX_DATA_2: begin
			    mosi_reg <= mosi_from_dp;   
			end
			
			SPIM_STATE_CAPT_RX_DATA: begin
                cnt_enable <= 1'b1;				
                sclk       <= ~pol_supprt;
			end
						
			SPIM_STATE_SEND_TX_DATA: begin
                if(dpMOSI_fifo_empty == 1'b1) begin				    
                    if(count[4:0] != 5'h02)begin
				        sclk     <= pol_supprt;
				        mosi_reg <= mosi_from_dp;				        
				    end 
				    else begin
				    	sclk <= pol_supprt;
						if(!ld_ident) begin
					        mosi_reg <= 1'b0;						    
                        end else begin
                            ld_ident <= 1'b0;	
                            mosi_reg <= mosi_from_dp;
						end							
					end
				end
				else begin				    
                 	sclk     <= pol_supprt;
				    mosi_reg <= mosi_from_dp;							
				end
			end
			
			SPIM_STATE_SHFT_N_LD_TX_DATA: begin
				sclk       <= pol_supprt;
				ld_ident   <= 1'b1;
				mosi_reg   <= mosi_from_dp;				
			end
            
            SPIM_STATE_SPI_DONE: begin
			    mosi_reg   <= 1'b0;
			    ss         <= 1'b1; 
				cnt_enable <= 1'b0;
				sclk       <= pol_supprt;
			end		
            
            default: begin
		        ss         <= 1'b1;
			    mosi_reg   <= 1'b0;
			    cnt_enable <= 1'b0; 
				ld_ident   <= 1'b0;
				sclk       <= pol_supprt;			    
            end		
		    endcase			
		end
	end 
	/* END of "CPHA == 0" State Machine implementation */
	
	endgenerate
    
	/* Couter7 instantiation */
	cy_psoc3_count7 #(.cy_period(BitCntPeriod),.cy_route_ld(1),.cy_route_en(1))
	BitCounter(
        /* input          */ .clock(clk_fin),
        /* input          */ .reset(reset),
        /* input          */ .load(1'b0),
        /* input          */ .enable(cnt_enable),
        /* output [06:00] */ .count(count),
        /* output         */ .tc(cnt_tc)
    );
	
	/* Statusi Register instantiation */
	cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h09), .cy_int_mask(7'h00))
    TxStsReg(
        /* input            */ .clock(clk_fin),
        /* input    [06:00] */ .status(tx_status),
        /* output           */ .interrupt(tx_interpt)
    );
	
    /* Statusi Register instantiation */
	cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h40), .cy_int_mask(7'h00))
    RxStsReg(
        /* input            */ .clock(clk_fin),
        /* input    [06:00] */ .status(rx_status),
        /* output           */ .interrupt(rx_interpt)
    );
    
	generate
	if(NumberOfDataBits <= SR8) begin: sR8
	
    cy_psoc3_dp8 #(.cy_dpconfig_a(dp8_cfg))
    Dp(
        /*  input           */ .clk(clk_fin),
        /* input            */ .reset(reset),  
        /*  input   [02:00] */ .cs_addr(state),
        /*  input           */ .route_si(miso_to_dp),
        /*  input           */ .route_ci(1'b0),
        /*  input           */ .f0_load(1'b0),
		/*  input           */ .f1_load(load_rx_data),
        /*  input           */ .d0_load(1'b0),
        /*  input           */ .d1_load(1'b0),
        /*  output          */ .ce0(),
        /*  output          */ .cl0(),
        /*  output          */ .z0(),
        /*  output          */ .ff0(),
        /*  output          */ .ce1(),
        /*  output          */ .cl1(),
        /*  output          */ .z1(),
        /*  output          */ .ff1(),
        /*  output          */ .ov_msb(),
        /*  output          */ .co_msb(),
        /*  output          */ .cmsb(),
        /*  output          */ .so(mosi_from_dp),
        /*  output          */ .f0_bus_stat(dpMOSI_fifo_not_full),
        /*  output          */ .f0_blk_stat(dpMOSI_fifo_empty),
        /*  output          */ .f1_bus_stat(dpMISO_fifo_not_empty),
        /*  output          */ .f1_blk_stat(dpMISO_fifo_full)
        );
    
	end /* NumberOfDataBits <= SR8 */
    else begin : sR16    /* NumberOfDataBits > 8 */
	cy_psoc3_dp16 #(.cy_dpconfig_a(dp16_msb_cfg), .cy_dpconfig_b(dp16_lsb_cfg))
    Dp(
            /*  input           */ .clk(clk_fin),
            /* input            */ .reset(reset),  
            /*  input   [02:00] */ .cs_addr(state),
            /*  input           */ .route_si(miso_to_dp),
            /*  input           */ .route_ci(1'b0),
            /*  input           */ .f0_load(1'b0),
            /*  input           */ .f1_load(load_rx_data),
            /*  input           */ .d0_load(1'b0),
            /*  input           */ .d1_load(1'b0),
            /*  output          */ .ce0(),
            /*  output          */ .cl0(),
            /*  output          */ .z0(),
            /*  output          */ .ff0(),
            /*  output          */ .ce1(),
            /*  output          */ .cl1(),
            /*  output          */ .z1(),
            /*  output          */ .ff1(),
            /*  output          */ .ov_msb(),
            /*  output          */ .co_msb(),
            /*  output          */ .cmsb(),
            /*  output          */ .so({mosi_from_dpL,mosi_from_dpR}),
            /*  output          */ .f0_bus_stat({dpMOSI_fifo_not_full, nc1}),
            /*  output          */ .f0_blk_stat({tmp1, nc2}),
            /*  output          */ .f1_bus_stat({dpMISO_fifo_not_empty, nc3}),
            /*  output          */ .f1_blk_stat({dpMISO_fifo_full,nc4})
    );
	    assign mosi_from_dp = (ShiftDir == SPIM_MSB_FIRST) ? mosi_from_dpL : mosi_from_dpR;
		assign dpMOSI_fifo_empty = tmp1;
	
    end /* NumberOfDataBits <= sR16 */
	endgenerate

endmodule
`endif /* B_SPIM_v1_50_V_ALREADY_INCLUDED */


