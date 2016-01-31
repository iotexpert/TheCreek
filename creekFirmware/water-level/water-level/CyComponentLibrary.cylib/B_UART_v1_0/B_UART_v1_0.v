/*******************************************************************************
 *
 * FILENAME:  B_UART_v1_0.v
 * COMPONENT NAME:   B_UART_v1_0
 * @Version@
 *
 * DESCRIPTION:
 *   	This file provides a top level model of the Base UART user module
 *      defining the controller and datapath instances and all of the necessary
 *      interconnect, for the RX and TX components individually.  This allows 
 *      for a lot of flexibility in how big the design can be and puts all of 
 *      the code in a single file for easy maintenance.
 *
 *
 *------------------------------------------------------------------------------
 *             TX   Data Path register definitions  
 *------------------------------------------------------------------------------
 *
 *  INSTANCE NAME:  dpTXShifter
 *
 *  DESCRIPTION:
 *    Data Shifter for the TX portion of the UART.
 *
 *  REGISTER USAGE:
 *   F0 => Data to be sent
 *   F1 => Unused
 *   D0 => Unused
 *   D1 => Unused
 *   A0 => Data as it is shifted out
 *   A1 => Unused
 *
 *------------------------------------------------------------------------------
 *             TX   7-Bit Counter Implementation Description  
 *------------------------------------------------------------------------------
 *
 *  INSTANCE NAME:  TXBitCounter
 *
 *  DESCRIPTION:
 *    Bit Clock Generator and Bit Counter rolled into a 7-Bit Counter.  Requires
 *      a period of the number of bits (+1 for the start bit) times the oversample rate
 *      of 8 or 16-bits.  It is required to use 8 or 16-bit oversampling because
 *      the lower 3 or 4 bits of the counter are masked and compared to zero to
 *      generate the bit clock.  Terminal count of the counter defines the end
 *      of a packet before Parity and Stop Bits are sent.
 *
 *  REGISTER USAGE:
 *   PERIOD => ((NumBits + 1) * (OverSample Rate)) - 1
 *
 *------------------------------------------------------------------------------
 *              RX  Data Path register definitions  
 *------------------------------------------------------------------------------
 *
 *  INSTANCE NAME:  dpRXBitClkGen
 *
 *  DESCRIPTION:
 *    Bit Clock Generator for the RX portion of the UART.
 *
 *  REGISTER USAGE:
 *   F0 => 
 *   F1 => 
 *   D0 => 
 *   D1 => 
 *   A0 => 
 *   A1 => 
 *
 *------------------------------------------------------------------------------
 *
 *  INSTANCE NAME:  dpRXBitCounter
 *
 *  DESCRIPTION:
 *    Bit Counter for the RX portion of the UART.
 *
 *  REGISTER USAGE:
 *   F0 => 
 *   F1 => 
 *   D0 => 
 *   D1 => 
 *   A0 => 
 *   A1 => 
 *
 *------------------------------------------------------------------------------
 *
 *  INSTANCE NAME:  dpRXShifter
 *  DATAFILE NAME:  dpRXShifter.dat
 *
 *  DESCRIPTION:
 *    Data Shifter for the RX portion of the UART.
 *
 *  REGISTER USAGE:
 *   F0 => Data just received
 *   F1 => Unused
 *   D0 => Unused
 *   D1 => Unused
 *   A0 => Data as it is shifted in
 *   A1 => Unused
 *
 *------------------------------------------------------------------------------
 *  IO SIGNALS:
 *   clock        input        User Supplied clock = 8x bitrate of TX and RX data
 *   reset        input        Global system reset signal
 *   rx           input        UART Serial data receive line                    <Compile option to include>
 *   tx           output       UART Serial data transmit line                   <Compile option to include>
 *   rts_n        input        UART Request to send control line                <Compile option to include>
 *   cts_n        output       UART Clear to send control line                  <Compile option to include>
 *   tx_en        output       UART Transmit enable control line                <Compile option to include>
 *
 *  Todo:
 *      * TX_COMPLETE status bit should only be sent if no new data is ready to be sent
 *      * Implement multiple sampling oversampling on RX.  Currently samples only once at 1/8th clock speed.
 *      * Check for defined implementation of hardware addressing
 *              Currently I simply indicate MARK or SPACE in the status register and assume that
 *              the firmware will check to see if the address is correct to know how to deal with 
 *              the next bytes of data, assuming it will toss out data until a new MARK is found
 *              where it can check the address again.
 *      * Need to implement x16 oversampling instead of 8 (could easily be 32 or 64 etc?  Already uses counter for it
 *      * Need to implement CTS when RX BUFFER Empty or not full signal is implemented with FIFO's.  Need to understand 
 *              how this really works, but it's implemented
 *      * Need to read-up on, understand and implement break status for the rx portion.
 *      * RX overrun status may not be correct for FIFO's yet.  Currently it just ands the fifo full with data ready. Data ready
 *              is based on the bit counter so it may not be reset correctly.  I need to simulate and do some research.
 *
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef B_UART_v1_0_V_ALREADY_INCLUDED
`else
`define B_UART_v1_0_V_ALREADY_INCLUDED
module B_UART_v1_0
(
    input 	wire clock,     /* Clock = 8x the bit-rate                        */
	input 	wire reset,     /* System Global Reset                            */
	input 	wire rx,        /* Recieve: Serial Data Input                     */
	output 	reg  tx,        /* Transmit: Serial Data Output                   */
	output	wire rx_interrupt,	/* Interrupt output from the RX section		  */
	output	wire tx_interrupt,	/* Interrupt output from the TX section		  */
    output 	wire cts_n,     /* Clear to send output for Flow control          */
    input 	wire rts_n,     /* Request to send input for Flow control         */
    output 	wire tx_en      /* Transmit Enable: Hardware control line output  */
);

/* DO NOT CHANGE these two parameters.  They define constants */
localparam UART_NUM_STOP_BITS_1 = 2'd1;
localparam UART_NUM_STOP_BITS_2 = 2'd2;

/* DO NOT CHANGE these two parameters.  They define constants */
localparam UART_OVER_SAMPLE_8    = 7'd8;
localparam UART_OVER_SAMPLE_16   = 7'd16;

/* DO NOT CHANGE these two parameters.  They define constants */
localparam UART_PARITY_TYPE_ODD     = 3'd1;
localparam UART_PARITY_TYPE_EVEN    = 3'd0;
localparam UART_PARITY_TYPE_MRKSPC  = 3'd2;
localparam UART_PARITY_TYPE_NONE    = 3'd3;
localparam UART_PARITY_TYPE_SW      = 3'd4;

/* DO NOT CHANGE these parameters.  They define constants */
localparam UART_RX_ADDR_MODE_NONE           = 8'd0;
localparam UART_RX_ADDR_MODE_SW_BYTE_BYTE   = 8'd1;
localparam UART_RX_ADDR_MODE_SW_DET_BUFFER  = 8'd2;
localparam UART_RX_ADDR_MODE_HW_BYTE_BYTE   = 8'd3;
localparam UART_RX_ADDR_MODE_HW_DET_BUFFER  = 8'd4;

/* These parameters will set by the software */
parameter [1:0] ParityType      = UART_PARITY_TYPE_ODD;
parameter FlowControl           = 1'b0;         /* Disabled by Default */
parameter HwTXEnSignal          = 1'b0;         /* Disabled by Default */
/* These two parameters will be set by software but are used to generate or remove blocks of code */
parameter RXEnable              = 1'b1;         /* Enabled RX portion of the UART by Default */
parameter TXEnable              = 1'b1;         /* Enabled TX portion of the UART by Default */

parameter [7:0] RXAddressMode   = UART_RX_ADDR_MODE_NONE;         /* Default is NONE */
parameter [7:0] Address1        = 8'd0;
parameter [7:0] Address2        = 8'd0;

parameter [6:0] NumDataBits           = 7'd8;         /* Set the Data Width from Software Parameter */
parameter [1:0] NumStopBits           = UART_NUM_STOP_BITS_2;
parameter [6:0] OverSampleCount       = UART_OVER_SAMPLE_8;   /* Allows Oversampling of 8 or 16-bits */
parameter [6:0] txperiod_init = ((NumDataBits+7'd1)*OverSampleCount)-7'd1;
parameter [6:0] ParityBitInit = (ParityType == UART_PARITY_TYPE_NONE) ? 7'd0 : 7'd1;
parameter [6:0] StopBits = (NumStopBits == UART_NUM_STOP_BITS_2) ? 7'd2 : 7'd1;
parameter [6:0] rxperiod_init = ((NumDataBits + ParityBitInit + StopBits)*OverSampleCount)+((OverSampleCount/2) - 7'd1);

/* DO NOT MODIFY because the datapath control store depends on these being constant */
/* TX State Machine States: */
localparam UART_TX_STATE_IDLE               = 3'd0;
localparam UART_TX_STATE_SEND_START         = 3'd1;
localparam UART_TX_STATE_SEND_DATA          = 3'd2;
localparam UART_TX_STATE_SEND_PARITY_ODDEV  = 3'd3;
localparam UART_TX_STATE_SEND_PARITY_MRKSPC = 3'd4;
localparam UART_TX_STATE_SEND_STOP1         = 3'd5;
localparam UART_TX_STATE_SEND_STOP2         = 3'd6;

/* Control Register bit locations */
localparam UART_CTRL_TXRUN               = 3'd0;
localparam UART_CTRL_RXRUN               = 3'd1;
localparam UART_CTRL_MARK                = 3'd2; /* 1 sets mark, 0 sets space */
localparam UART_CTRL_PARITYTYPE0         = 3'd3; /* Defines the type of parity implemented */
localparam UART_CTRL_PARITYTYPE1         = 3'd4; /* Defines the type of parity implemented */
localparam UART_CTRL_RXADDR_MODE0        = 3'd5;
localparam UART_CTRL_RXADDR_MODE1        = 3'd6;
localparam UART_CTRL_RXADDR_MODE2        = 3'd7;

localparam UART_RX_CTRL_ADDR_MODE_NONE           = 3'd0;
localparam UART_RX_CTRL_ADDR_MODE_SW_BYTE_BYTE   = 3'd1;
localparam UART_RX_CTRL_ADDR_MODE_SW_DET_BUFFER  = 3'd2;
localparam UART_RX_CTRL_ADDR_MODE_HW_BYTE_BYTE   = 3'd3;
localparam UART_RX_CTRL_ADDR_MODE_HW_DET_BUFFER  = 3'd4;

/* TX Status Register bit locations */
localparam UART_TX_STS_TX_COMPLETE          = 8'h0;
localparam UART_TX_STS_TX_BUFF_EMPTY        = 8'h1;
localparam UART_TX_STS_TX_FIFO_FULL         = 8'h2;
localparam UART_TX_STS_TX_FIFO_NOT_FULL     = 8'h3;

/* Parameters used for resource consumption Control,  Setting them to 1 means use the Datapath instead of the 7-bit counter */
parameter  TXCounterDP = 0;    
parameter  RXCounterDP = 0;
parameter  Use23Polling = 1;
parameter  TXBitClkGenDP = 0;

reg     [2:0]   tx_state;               /* Transmit State Machine State Container */
wire            TXcounter_tc;

wire        tx_interrupt_out; 
wire        rx_interrupt_out; 
assign      tx_interrupt = (TXEnable==1) ? tx_interrupt_out : 1'b0;
assign      rx_interrupt = (RXEnable==1) ? rx_interrupt_out : 1'b0;

wire [7:0]  control;             /* UART Control Register */
/* Instantiate the control register */
cy_psoc3_control
	#(.cy_force_order(1))
ctrl(
    /*  output	[07:00]	         */  .control(control)
);

/* TX Controller Logic */
generate
if (TXEnable == 1) begin : sTX	
	wire [6:0]  tx_status;              /* Transmit portion of the UART Status Register */
	wire        tx_fifo_empty;          /* Transmitter FIFO Empty status line from FIFO - used for flow control of data ready to send */
	wire        tx_fifo_notfull;        /* Transmitter FIFO not full status line from FIFO - Firmware should check the status bit FIFO_FULL before writing more data */
	reg         tx_parity_bit;          /* Transmitter Parity Bit container (sent on TX during parity bit state) */
    //TODO: 7-bit counter workaround requires a tx_bitclk_pre wire and tx_bitclk reg  
	wire        tx_bitclk_pre;              /* Transmitter bit Clock (1/8 system clock) */
    reg         tx_bitclk;
    
    wire            tx_ctrl_run;
    wire            tx_ctrl_mark;
    wire    [2:0]   tx_ctrl_addr_mode;
    
    assign  tx_ctrl_run = control[UART_CTRL_TXRUN];
    assign  tx_ctrl_mark = control[UART_CTRL_MARK];
    assign  tx_ctrl_addr_mode = {control[UART_CTRL_RXADDR_MODE2], control[UART_CTRL_RXADDR_MODE1], control[UART_CTRL_RXADDR_MODE0]};
    
    wire    [1:0] TXFinalParityType;
    if(ParityType == UART_PARITY_TYPE_SW) begin : sTXParity
        wire    [1:0]   tx_ctrl_paritytype;
        assign  tx_ctrl_paritytype = {control[UART_CTRL_PARITYTYPE1], control[UART_CTRL_PARITYTYPE0]};  
        assign  TXFinalParityType = tx_ctrl_paritytype;
    end
    else begin
        assign TXFinalParityType = ParityType[1:0];
    end
    
    reg             tx_mark;
    always @(posedge tx_bitclk or posedge reset) begin
        if(reset) begin
            tx_mark <= 1'b0;
        end
        else begin
            if((tx_state == UART_TX_STATE_SEND_START) && (TXFinalParityType == UART_PARITY_TYPE_MRKSPC)) begin
                tx_mark <= tx_ctrl_mark & !tx_mark;
            end
            else
                tx_mark <= 1'b0;
        end
    end

	cy_psoc3_dp8 #(.cy_dpconfig_a (
	{
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: IDLE*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: SEND START*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP___SR, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: SEND DATA (SR)*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: SEND PARITY (ODD/EVEN)*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: SEND PARITY (MRK/SPC)*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: SEND STOP*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
		8'hFF, 8'h00,	/* SC_REG4	Comment: */
		8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
		`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
		`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
		`SC_SI_A_DEFSI, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, `SC_SHIFT_SR, 1'b0,
		1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
		`SC_MSB_ENBL, `SC_MSB_BIT7, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
	}), 
    .d0_init_a(Address1),
    .d1_init_a(Address2))
	dpTXShifter (
	/*  input                   */  .clk(tx_bitclk),
	/*  input   [02:00]         */  .cs_addr(tx_state),
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
	/*  output                  */  .so(tx_shift_out),
	/*  output                  */  .f0_bus_stat(tx_fifo_notfull), //FIFO is not full when bus_stat = 1;
	/*  output                  */  .f0_blk_stat(tx_fifo_empty), //FIFO is empty when blk_stat = 1;
	/*  output                  */  .f1_bus_stat(),
	/*  output                  */  .f1_blk_stat()
	);
	
    wire [6:0] txbitcount;
    assign tx_bitclk_pre = txbitcount[2:0] == 3'd0;
    wire counter_load = tx_state == UART_TX_STATE_IDLE;
    cy_psoc3_count7 #(.cy_period(txperiod_init),.cy_route_ld(1),.cy_route_en(1)) 
    TXBitCounter(
        /*  input		     */  .clock(clock),
        /*  input		     */  .reset(reset),
        /*  input		     */  .load(counter_load),
        /*  input		     */  .enable(tx_ctrl_run),
        /*  output	[06:00]	 */  .count(txbitcount),
        /*  output		     */  .tc(TXcounter_tc)
    );
    
	
	assign tx_status[6:4] = 3'b0;
	assign tx_status[UART_TX_STS_TX_COMPLETE] = tx_fifo_empty & ((NumStopBits == UART_NUM_STOP_BITS_1) ? (tx_state == UART_TX_STATE_SEND_STOP1) : (tx_state == UART_TX_STATE_SEND_STOP2));
	assign tx_status[UART_TX_STS_TX_BUFF_EMPTY] = tx_fifo_empty;
	assign tx_status[UART_TX_STS_TX_FIFO_FULL] = !tx_fifo_notfull;
    assign tx_status[UART_TX_STS_TX_FIFO_NOT_FULL] = tx_fifo_notfull;
	
    /* Instantiate the status register and interrupt hook*/
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h01), 
        .cy_int_mask(7'h7F)) 
    tx_sts(
        /* input          */  .clock(clock),
        /* input  [06:00] */  .status(tx_status),
        /* output         */  .interrupt(tx_interrupt_out)
    );
	
	assign tx_en =  ( HwTXEnSignal && (tx_state != UART_TX_STATE_IDLE)) ? 1'b1 : 1'b0;
    
    //TODO: Workaround for TC of 7-bit counter not being at 0 but at the rollover point 1 clock cycle later
    always @(posedge clock) begin
        tx_bitclk <= tx_bitclk_pre;
    end
    
	/* TX Controller Logic */
	always @(posedge clock or posedge reset) 
    begin
		if(reset) 
        begin
			tx_state <= UART_TX_STATE_IDLE;
            tx <= 1'b1;
		end
		else 
        begin
			case(tx_state)
				UART_TX_STATE_IDLE: begin
                    tx <= 1'b1;
					if(rts_n && FlowControl) begin
						tx_state <= UART_TX_STATE_IDLE;
					end
					else begin
						if(!tx_fifo_empty) begin
							tx_state <= UART_TX_STATE_SEND_START;
						end
                        else begin
                            tx_state <= UART_TX_STATE_IDLE;
                        end
					end
				end
				UART_TX_STATE_SEND_START: begin
					tx_parity_bit <= control[UART_CTRL_PARITYTYPE0] && ((TXFinalParityType == UART_PARITY_TYPE_ODD) || (TXFinalParityType == UART_PARITY_TYPE_EVEN));
                    tx <= 1'b0;
                    if(tx_bitclk) begin
                        tx <= tx_shift_out;
                        tx_state <= UART_TX_STATE_SEND_DATA;
                    end
                    else begin
                        tx_state <= UART_TX_STATE_SEND_START;
                    end
				end
				UART_TX_STATE_SEND_DATA: begin
                    tx_parity_bit <= (tx_parity_bit ^ tx_shift_out) && ((TXFinalParityType == UART_PARITY_TYPE_ODD) || (TXFinalParityType == UART_PARITY_TYPE_EVEN));
                    tx <= tx;
                    if(tx_bitclk) begin
                        if(TXcounter_tc) begin
    						if(TXFinalParityType == UART_PARITY_TYPE_EVEN || TXFinalParityType == UART_PARITY_TYPE_ODD) begin
    							tx_state <= UART_TX_STATE_SEND_PARITY_ODDEV;
                                tx <= tx_parity_bit;
    						end
    						else if (TXFinalParityType == UART_PARITY_TYPE_MRKSPC) begin
    							tx_state <= UART_TX_STATE_SEND_PARITY_MRKSPC;
                                tx <= tx_mark;
    						end
    						else begin
    							tx_state <= UART_TX_STATE_SEND_STOP1;
                                tx <= 1'b1;
    						end
    					end
                        else begin
                            tx <= tx_shift_out;
                            tx_state <= UART_TX_STATE_SEND_DATA;
                        end
                    end
                    else begin
                        tx_state <= UART_TX_STATE_SEND_DATA;
                    end                    
				end
				UART_TX_STATE_SEND_PARITY_ODDEV: begin
                    tx <= 1'b1;
                    if(tx_bitclk) begin
                        tx_state <= UART_TX_STATE_SEND_STOP1;
					end
                    else begin
                        tx_state <= UART_TX_STATE_SEND_PARITY_ODDEV;
                    end
				end
				UART_TX_STATE_SEND_PARITY_MRKSPC: begin
                    tx <= 1'b1;
                    if(tx_bitclk) begin
                        tx_state <= UART_TX_STATE_SEND_STOP1;
                    end
                    else begin
                        tx_state <= UART_TX_STATE_SEND_PARITY_MRKSPC;
                    end
				end
				UART_TX_STATE_SEND_STOP1: begin
                    tx <= 1'b1;
                    if(tx_bitclk) begin
    					if(NumStopBits == UART_NUM_STOP_BITS_1) begin
                            tx_state <= UART_TX_STATE_IDLE;
    					end
    					else begin
                            tx_state <= UART_TX_STATE_SEND_STOP2;
    					end
                    end
                    else begin
                        tx_state <= UART_TX_STATE_SEND_STOP1;
					end
				end
                UART_TX_STATE_SEND_STOP2: begin
                    tx <= 1'b1;
                    if(tx_bitclk) begin
    					tx_state <= UART_TX_STATE_IDLE;
                    end
                    else begin
                        tx_state <= UART_TX_STATE_SEND_STOP2;
                    end
				end
			endcase
		end /* end of else statement */
	end /* End of always block */
end /* End of TXEnable Generate Statement */
endgenerate /* sTX */



      
/* DO NOT MODIFY because the datapath control store depends on these being constant */
/* RX State Machine States: */
localparam UART_RX_STATE_IDLE              = 3'd0;
localparam UART_RX_STATE_CHECK_START       = 3'd1;
localparam UART_RX_STATE_GET_DATA          = 3'd2;
localparam UART_RX_STATE_GET_PARITY_ODDEV  = 3'd3;
localparam UART_RX_STATE_GET_PARITY_MRKSPC = 3'd4;
localparam UART_RX_STATE_CHECK_STOP1       = 3'd5;

/* RX Status Register bit locations (Currently defined as constants DO NOT CHANGE) */
localparam UART_RX_STS_MRKSPC             = 3'd0;
localparam UART_RX_STS_BREAK              = 3'd1;
localparam UART_RX_STS_PAR_ERROR          = 3'd2;
localparam UART_RX_STS_STOP_ERROR         = 3'd3;
localparam UART_RX_STS_OVERRUN            = 3'd4;
localparam UART_RX_STS_FIFO_NOTEMPTY      = 3'd5;
localparam UART_RX_STS_ADDR_MATCH         = 3'd6;
	  
/* RX Controller Logic */
generate
	if (RXEnable == 1) 	begin:sRX    
		wire [6:0]  rx_status;
		reg  [2:0]  rx_state;
		reg         rx_parity_bit;
		reg         rx_markspace_status;
		reg         rx_parity_error_status;
		reg         rx_break_status;
		reg         rx_stop_bit_error;
        wire        rx_postpoll;
        wire        rx_load_fifo;
        wire        rx_fifofull;
        wire        rx_fifonotempty;
        wire        rx_addressmatch1;        
        wire        rx_addressmatch2;
        wire [6:0]  rx_count;
        wire        rx_counter_load;
        wire        rx_bitclk_pre;
        reg         rx_bitclk;
        wire [6:0]  NumExtraBits;
        wire [6:0]  ParityBits;
        
        assign NumExtraBits = StopBits + ParityBits;
        wire    rx_ctrl_run;
        wire    [1:0] rx_ctrl_paritytype;
        wire    [2:0] rx_ctrl_addr_mode;
        assign  rx_ctrl_run = control[UART_CTRL_RXRUN];        
        assign  rx_ctrl_addr_mode = {control[UART_CTRL_RXADDR_MODE2], control[UART_CTRL_RXADDR_MODE1], control[UART_CTRL_RXADDR_MODE0]};
        
        wire    [1:0] RXFinalParityType;
        if(ParityType == UART_PARITY_TYPE_SW) begin : sRXParity
            wire    [1:0]   rx_ctrl_paritytype;
            assign  rx_ctrl_paritytype = {control[UART_CTRL_PARITYTYPE1], control[UART_CTRL_PARITYTYPE0]};  
            assign  RXFinalParityType = rx_ctrl_paritytype;
            assign  ParityBits = (RXFinalParityType == UART_PARITY_TYPE_NONE) ? 7'd0 : 7'd1;
        end
        else begin
            assign RXFinalParityType = ParityType[1:0];
            assign ParityBits = (ParityType == UART_PARITY_TYPE_NONE) ? 7'd0 : 7'd1;
        end
		
        localparam [2:0] rxmsbsel = (NumDataBits == 8 || NumDataBits == 9) ? 3'd7 :
                                    (NumDataBits == 7) ? 3'd6 :
                                    (NumDataBits == 6) ? 3'd5 :
                                  /*(NumDataBits == 5) */ 3'd4;
        localparam [7:0] addressMask = (NumDataBits == 8 || NumDataBits == 9) ? 8'hFF :
                                    (NumDataBits == 7) ? 8'h7F :
                                    (NumDataBits == 6) ? 8'h3F :
                                  /*(NumDataBits == 5) */ 8'h1F;
        localparam [7:0] dataMask = (NumDataBits == 8 || NumDataBits == 9) ? 8'hFF :
                                    (NumDataBits == 7) ? 8'h7F :
                                    (NumDataBits == 6) ? 8'h3F :
                                  /*(NumDataBits == 5) */ 8'h1F;
        cy_psoc3_dp8 #(.cy_dpconfig_a (
		{
			`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
			`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
			`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
			`CS_CMP_SEL_CFGA, /*CS_REG0 Comment: IDLE*/
			`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
			`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
			`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
			`CS_CMP_SEL_CFGA, /*CS_REG1 Comment: CHECK START*/
			`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
			`CS_SHFT_OP___SR, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
			`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
			`CS_CMP_SEL_CFGA, /*CS_REG2 Comment: GET DATA (SR)*/
			`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
			`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
			`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
			`CS_CMP_SEL_CFGA, /*CS_REG3 Comment: GET PARITY (ODD/EVEN)*/
			`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
			`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
			`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
			`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: GET PARITY (MRK/SPC)*/
			`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
			`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
			`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
			`CS_CMP_SEL_CFGA, /*CS_REG5 Comment: CHECK STOP*/
			`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
			`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
			`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
			`CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
			`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
			`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
			`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
			`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
			dataMask, 8'h00,	/*SC_REG4	Comment: */
			8'hFF, addressMask,	/*SC_REG5	Comment: */
			`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
			`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_ENBL,
			`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
			`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
			`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
			1'b0, `SC_FIFO1__A0, `SC_FIFO0__A0,
			`SC_MSB_ENBL, rxmsbsel, `SC_MSB_NOCHN,
			`SC_FB_NOCHN, `SC_CMP1_NOCHN,
			`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
			10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
		}))
		dpRXShifter (
		/*  input                   */  .clk(rx_bitclk),
		/*  input   [02:00]         */  .cs_addr(rx_state),
		/*  input                   */  .route_si(rx_postpoll),
		/*  input                   */  .route_ci(1'b0),
		/*  input                   */  .f0_load(rx_load_fifo),
		/*  input                   */  .f1_load(1'b0),
		/*  input                   */  .d0_load(1'b0),
		/*  input                   */  .d1_load(1'b0),
		/*  output                  */  .ce0(rx_addressmatch1),
		/*  output                  */  .cl0(),
		/*  output                  */  .z0(),
		/*  output                  */  .ff0(),
		/*  output                  */  .ce1(rx_addressmatch2),
		/*  output                  */  .cl1(),
		/*  output                  */  .z1(),
		/*  output                  */  .ff1(),
		/*  output                  */  .ov_msb(),
		/*  output                  */  .co_msb(),
		/*  output                  */  .cmsb(),
		/*  output                  */  .so(),
		/*  output                  */  .f0_bus_stat(rx_fifonotempty), /* FIFO is not EMPTY */
		/*  output                  */  .f0_blk_stat(rx_fifofull),/* FIFO is FULL */
		/*  output                  */  .f1_bus_stat(),
		/*  output                  */  .f1_blk_stat()
		);
    
    assign rx_counter_load = (rx_state == UART_RX_STATE_IDLE) ? 1'b1 : 1'b0;
    assign rx_bitclk_pre = (rx_count[2:0] == 3'd0) ? 1'b1 : 1'b0;
    cy_psoc3_count7 #(.cy_period(rxperiod_init),.cy_route_ld(1),.cy_route_en(1)) 
        RXBitCounter(
        /*  input		     */  .clock(clock),
        /*  input		     */  .reset(reset),
        /*  input		     */  .load(rx_counter_load),
        /*  input		     */  .enable(rx_ctrl_run),
        /*  output	[06:00]	 */  .count(rx_count),
        /*  output		     */  .tc()
        );
    
    always @(posedge clock) begin
        rx_bitclk <= rx_bitclk_pre;
    end
    
    if(Use23Polling) begin : s23Poll
        reg [1:0] pollcount;
        reg       pollrxbit;
        wire      pollingrange;
        wire      rx_poll_bit0;
        wire      rx_poll_bit1;
        wire      rx_poll_bit2;
        
        assign rx_poll_bit0 = (rx_count[2:0] == 3'd2) ? 1'b1 : 1'b0;
        assign rx_poll_bit1 = (rx_count[2:0] == 3'd1) ? 1'b1 : 1'b0;
        assign rx_poll_bit2 = rx_bitclk_pre;
        assign pollingrange = rx_poll_bit0 | rx_poll_bit1 | rx_poll_bit2 | rx_bitclk;
        
        always @(posedge clock or posedge reset) begin
            if(reset) begin
                pollcount <= 2'd0;
            end
            else begin
                if(pollingrange) begin
                    if(rx) begin
                        pollcount <= pollcount + 2'd1;
                    end
                end
                else begin
                    pollcount <= 2'd0;
                end
            end
        end
        
        assign rx_postpoll = (pollcount < 2'd2) ? 1'b0 : 1'b1;
    end
    else begin
        assign rx_postpoll = rx;
    end
		/* Address Modes define when this bit is set */
        /* Address Mode 00 = Software Byte by Byte - Generate Interrupt when address byte detected */
        /* Address Mode 01 = Software Address To Buffer - Generate Interrupt when Address byte detected */
        /* Address mode 10 = Hardware Byte by Byte - Generate Interrupt when address byte is detected and matches one of the addresses */
        /* Address mode 10 = Hardware Address To Buffer - Generate Interrupt when address byte is detected and matches one of the addresses */
        //TODO: Isn't there supposed to be two addresses to match
		assign rx_status[UART_RX_STS_MRKSPC] = (rx_ctrl_addr_mode == UART_RX_ADDR_MODE_HW_BYTE_BYTE) ? (rx_markspace_status & (rx_addressmatch1 | rx_addressmatch2)) :
                                               (rx_ctrl_addr_mode == UART_RX_ADDR_MODE_HW_DET_BUFFER) ? (rx_markspace_status & (rx_addressmatch1 | rx_addressmatch2)) :
                                               (rx_ctrl_addr_mode == UART_RX_ADDR_MODE_NONE) ? (1'b0) :
                                               rx_markspace_status;
		assign rx_status[UART_RX_STS_PAR_ERROR] = (rx_parity_error_status & rx_load_fifo) && (RXFinalParityType != UART_PARITY_TYPE_NONE);    /* Parity Error */
		assign rx_status[UART_RX_STS_BREAK] = rx_break_status;                              /* Not Currently Implemented */
		assign rx_status[UART_RX_STS_STOP_ERROR]  = rx_stop_bit_error & rx_load_fifo;       /* Framing Error */
		assign rx_status[UART_RX_STS_OVERRUN] = rx_fifofull & rx_load_fifo; 
		assign rx_status[UART_RX_STS_FIFO_NOTEMPTY] = rx_fifonotempty;
        assign rx_status[UART_RX_STS_ADDR_MATCH] = (RXAddressMode  == UART_RX_ADDR_MODE_NONE) ? 1'b0 : ((rx_addressmatch1 | rx_addressmatch2) & rx_load_fifo);
        
        assign rx_load_fifo = (rx_state == UART_RX_STATE_CHECK_STOP1);
        
		assign cts_n = rx_fifofull;
		
        /* Instantiate the status register and interrupt hook*/
        cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h5F), 
            .cy_int_mask(7'h7F)) 
        rx_sts(
            /* input          */  .clock(rx_bitclk),
            /* input  [06:00] */  .status(rx_status),
            /* output         */  .interrupt(rx_interrupt_out)
        );
	
		always @(posedge clock or posedge reset) begin         /* Doesn't run off of bit clock */
			if(reset) begin
				rx_stop_bit_error <= 1'b0;
				rx_markspace_status <= 1'b0;
				rx_parity_error_status <= 1'b0;
				rx_break_status <= 1'b0;
				rx_state <= UART_RX_STATE_IDLE;
			end
			else begin
                    rx_stop_bit_error <= rx_stop_bit_error;
					rx_markspace_status <= rx_markspace_status && (RXFinalParityType == UART_PARITY_TYPE_MRKSPC);
					rx_parity_error_status <= rx_parity_error_status;
					rx_break_status <= rx_break_status;
				case(rx_state)
					UART_RX_STATE_IDLE: begin
						rx_stop_bit_error <= 1'b0;
                        rx_markspace_status <= 1'b0;
                        rx_parity_error_status <= 1'b0;
                        rx_break_status <= 1'b0;
						if(~rx) begin
                            rx_state <= UART_RX_STATE_CHECK_START;
                        end
                        else begin
                            rx_state <= UART_RX_STATE_IDLE;
                        end
						rx_parity_bit <= control[UART_CTRL_PARITYTYPE0];
					end
					UART_RX_STATE_CHECK_START: begin /* Check the start bit after 4 cycles */
						if(rx_bitclk) begin
							if(rx) begin
								rx_state <= UART_RX_STATE_IDLE;    
							end
							else begin
                                rx_state <= UART_RX_STATE_GET_DATA;
                            end
						end
					end
					UART_RX_STATE_GET_DATA:  begin  /* Get the data bits */
						if(rx_bitclk) begin
							rx_parity_bit <= (rx_parity_bit ^ rx);
						end
						if(rx_count[6:3] < NumExtraBits[2:0]) begin      //TODO: 4:3 cannot be magic numbers.  They must be calculated based on OverSamplingRate
							if(RXFinalParityType == UART_PARITY_TYPE_EVEN || RXFinalParityType == UART_PARITY_TYPE_ODD) begin
								rx_state <= UART_RX_STATE_GET_PARITY_ODDEV;
							end
							else if(RXFinalParityType == UART_PARITY_TYPE_MRKSPC) begin
								rx_state <= UART_RX_STATE_GET_PARITY_MRKSPC;
							end
							else begin
								rx_state <= UART_RX_STATE_CHECK_STOP1;
							end
						end
                        else begin
                            rx_state <= UART_RX_STATE_GET_DATA;
                        end
					end
					UART_RX_STATE_GET_PARITY_ODDEV: begin
						if(rx_bitclk) begin
							if(rx != rx_parity_bit) begin
								rx_parity_error_status <= 1'b1;
							end
							rx_state <= UART_RX_STATE_CHECK_STOP1;
						end
                        else begin
                            rx_state <= UART_RX_STATE_GET_PARITY_ODDEV;
                        end
					end	/* UART_RX_STATE_GET_PARITY_ODDEV*/
					UART_RX_STATE_GET_PARITY_MRKSPC: begin
						if(rx_bitclk) begin
							rx_markspace_status <= rx && (RXFinalParityType == UART_PARITY_TYPE_MRKSPC);
							/*Generate Mark Space Interrupt?  Should be enabled in Status register interrupt configuration */
							rx_state <= UART_RX_STATE_CHECK_STOP1;
						end
                        else begin
                            rx_state <= UART_RX_STATE_GET_PARITY_MRKSPC;
                        end
					end /* UART_RX_STATE_GET_PARITY_MRKSPC */
					UART_RX_STATE_CHECK_STOP1: begin
						if(rx_bitclk) begin
							if(~rx) begin
								rx_stop_bit_error <= 1'b1;   /*Set Error Flag */
                                rx_state <= UART_RX_STATE_IDLE;
							end
                            else begin
                                rx_state <= UART_RX_STATE_IDLE;
                            end
						end
                        else begin
                            rx_state <= UART_RX_STATE_CHECK_STOP1;
                        end
					end /*UART_RX_STATE_CHECK_STOP1 */
				endcase
			end /* End of Else Statement */
		end /* End of Always Block */
	end    /* End of RXEnable Generate Statement begin*/
endgenerate /* sRX */

endmodule /* B_UART_v1_0 */

`endif /* B_UART_v1_0_V_ALREADY_INCLUDED */
