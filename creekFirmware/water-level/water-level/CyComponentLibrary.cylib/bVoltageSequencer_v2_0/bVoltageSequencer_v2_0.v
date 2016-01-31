/*******************************************************************************
* File Name: bVoltageSequencer_v2_0.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* DESCRIPTION:
*   The VoltageSequencer is primarily a firmware driven component. The verilog
*   file serves 3 purposes:
*
*      1) Instantiates control registers to drive the converter enable pins
*         and status registers to read the converter pgood outputs.
*
*      2) To implement Rapid Fault Response Hardware Block. The intention of
*         this block is turning off a power converter in response to a fault
*         condition in the shortest time possible.
*
*      3) To drive the interrupt terminal on the symbol (buried) for Fault
*         ISR processing.
*
********************************************************************************
*                 I*O Signals:
********************************************************************************
*    Name           Direction       Description
*    clock          input           Input component clock
*    enable         input           Global enable input 
*    ctl[6:1]       input           General purpose control signals    
*    pgood1..16     input           Converter pgood status
*    sys_stable     output          System power stability status
*    sys_up         output          System power ON status
*    sys_dn         output          System power OFF status
*    warn           output          System power warning status
*    fault          output          System power fault status
*    sts[6:1]       output          General purpose outputs
*    en1..16        output          Converter enable control
*    interrupt      output          Interrupt terminal
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*********************************************************************************/
 
`include "cypress.v"
`ifdef bVoltageSequencer_v2_0_V_ALREADY_INCLUDED
`else
`define bVoltageSequencer_v2_0_V_ALREADY_INCLUDED

module bVoltageSequencer_v2_0 (
	input   wire        clock,          /* Input clock */
    input   wire        systick,        /* Timing source for tick time */
	input   wire [6:1]  ctl,            /* General purpose control inputs */
	input   wire        enable,         /* Global enable input */
	input   wire        pgood1,         /* Converter  1 pgood status */
	input   wire        pgood2,         /* Converter  2 pgood status */
	input   wire        pgood3,         /* Converter  3 pgood status */
	input   wire        pgood4,         /* Converter  4 pgood status */
	input   wire        pgood5,         /* Converter  5 pgood status */
	input   wire        pgood6,         /* Converter  6 pgood status */
	input   wire        pgood7,         /* Converter  7 pgood status */
	input   wire        pgood8,         /* Converter  8 pgood status */
	input   wire        pgood9,         /* Converter  9 pgood status */
	input   wire        pgood10,        /* Converter 10 pgood status */
	input   wire        pgood11,        /* Converter 11 pgood status */
	input   wire        pgood12,        /* Converter 12 pgood status */
	input   wire        pgood13,        /* Converter 13 pgood status */
	input   wire        pgood14,        /* Converter 14 pgood status */
	input   wire        pgood15,        /* Converter 15 pgood status */
	input   wire        pgood16,        /* Converter 16 pgood status */
	input   wire        pgood17,        /* Converter 17 pgood status */
	input   wire        pgood18,        /* Converter 18 pgood status */
	input   wire        pgood19,        /* Converter 19 pgood status */
	input   wire        pgood20,        /* Converter 20 pgood status */
	input   wire        pgood21,        /* Converter 21 pgood status */
	input   wire        pgood22,        /* Converter 22 pgood status */
	input   wire        pgood23,        /* Converter 23 pgood status */
	input   wire        pgood24,        /* Converter 24 pgood status */
	input   wire        pgood25,        /* Converter 25 pgood status */
	input   wire        pgood26,        /* Converter 26 pgood status */
	input   wire        pgood27,        /* Converter 27 pgood status */
	input   wire        pgood28,        /* Converter 28 pgood status */
	input   wire        pgood29,        /* Converter 29 pgood status */
	input   wire        pgood30,        /* Converter 30 pgood status */
	input   wire        pgood31,        /* Converter 31 pgood status */
	input   wire        pgood32,        /* Converter 32 pgood status */
	output  wire        fault,          /* System power fault status */
    output  wire        warn,           /* System power warning status */
	output  wire [6:1]  sts,            /* General purpose status signals */
	output  wire        sys_dn,         /* System power OFF status */
	output  wire        sys_stable,     /* System power stability status */
	output  wire        sys_up,         /* System power OFF status */
    output  wire        en1,            /* Converter  1  enable control */
	output  wire        en2,            /* Converter  2  enable control */
	output  wire        en3,            /* Converter  3  enable control */
	output  wire        en4,            /* Converter  4  enable control */
	output  wire        en5,            /* Converter  5  enable control */
	output  wire        en6,            /* Converter  6  enable control */
	output  wire        en7,            /* Converter  7  enable control */
	output  wire        en8,            /* Converter  8  enable control */
	output  wire        en9,            /* Converter  9  enable control */
	output  wire        en10,           /* Converter 10  enable control */
	output  wire        en11,           /* Converter 11  enable control */
	output  wire        en12,           /* Converter 12  enable control */
	output  wire        en13,           /* Converter 13  enable control */
	output  wire        en14,           /* Converter 14  enable control */
	output  wire        en15,           /* Converter 15  enable control */
	output  wire        en16,           /* Converter 16  enable control */
	output  wire        en17,           /* Converter 17  enable control */
	output  wire        en18,           /* Converter 18  enable control */
	output  wire        en19,           /* Converter 19  enable control */
	output  wire        en20,           /* Converter 20  enable control */
	output  wire        en21,           /* Converter 21  enable control */
	output  wire        en22,           /* Converter 22  enable control */
	output  wire        en23,           /* Converter 23  enable control */
	output  wire        en24,           /* Converter 24  enable control */
	output  wire        en25,           /* Converter 25  enable control */
	output  wire        en26,           /* Converter 26  enable control */
	output  wire        en27,           /* Converter 27  enable control */
	output  wire        en28,           /* Converter 28  enable control */
	output  wire        en29,           /* Converter 29  enable control */
	output  wire        en30,           /* Converter 30  enable control */
	output  wire        en31,           /* Converter 31  enable control */
	output  wire        en32,           /* Converter 32  enable control */
    output  wire        seq_tick,       /* 250 us tick timer */
    output  wire        stable_tick,    /* 8 ms tick timer */
    output  reg         fault_interrupt /* Fault detection interrupt */
);

    /***************************************************************************
    *                       Parameters
    ***************************************************************************/
    parameter [5:0] NumConverters = 6'd8;  /* Number of converters */
    parameter [2:0] NumCtlInputs  = 3'd1;  /* Number of control inputs */
    parameter [2:0] NumStsOutputs = 3'd1;  /* Number of status outputs */
    
    /* Combine pgood[x] into bus */
    wire [32:1] pgood_bus = {pgood32, pgood31, pgood30, pgood29, pgood28, pgood27, pgood26, pgood25,
                             pgood24, pgood23, pgood22, pgood21, pgood20, pgood19, pgood18, pgood17,
                             pgood16, pgood15, pgood14, pgood13, pgood12, pgood11, pgood10, pgood9,
                             pgood8,  pgood7,  pgood6,  pgood5,  pgood4,  pgood3,  pgood2,  pgood1};

    /***************************************************************************
    *               Instantiation of udb_clock_enable primitive 
    ****************************************************************************
    * The udb_clock_enable primitive component allows to support clock enable 
    * mechanism and specify the intended synchronization behavior for the clock 
    * result (operational clock).
    */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(op_clk)
    );

    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) TickSync
    (
        /* input  */    .clock_in(systick),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(op_systick)
    );
    /***************************************************************************
    *   Sequencer State Machine and System Stable Timer Tick 
    ****************************************************************************
    * To implement the 250 us tick timer interrupt used to drive state machine
    * transitions and 250 us and the 8 ms tick timer used to count System Stable
    * a single count7 is required. 
    * The count7 divides the input 8kHz clock frequency by 2 to generate 250 us
    * tick (count0) and by 64 to generate 8 ms tick (tc).
    ***************************************************************************/
    wire [6:0]  count; 
    cy_psoc3_count7 #(.cy_period(7'd63)) TickTimer
    (
        /*  input             */  .clock(op_systick),
        /*  input             */  .reset(1'b0),
        /*  input             */  .load(1'b0),
        /*  input             */  .enable(1'b1),
        /*  output  [06:00]   */  .count(count),
        /*  output            */  .tc(stable_tick)
    );
    assign seq_tick = count[0];
    
    /***************************************************************************
    *           Status Register to monitor enable and ctl[6:0] inputs                    
    ****************************************************************************
    * Hardware monitoring of fault condition due to a de-assertion of ctl[6:1]
    * inputs is implemented using built-in logic of StatusI register. 
    * The polarity of ctl[6:1] is user defined and can be changed from 
    * firmware. Fault detection on one ctl[x] input is implemented with the 
    * following logic equation: fault = ~ctl[x] & pol | ctl[x] & ~pol.
    * One statusi register can be used to monitor 3 ctl[x] inputs.
    ***************************************************************************/
    wire ctl_flt1;
    wire ctl_flt2;
    
    cy_psoc3_statusi #(.cy_force_order(`TRUE), .cy_md_select(7'h0), .cy_int_mask(7'h0)) Ctl_Mon1
    (
        /* input         */ .clock(op_clk),
        /* input [06:00] */ .status({enable, ctl}),
        /* output        */ .interrupt(ctl_flt1)
    );    

    generate
    if (NumCtlInputs > 3'd0)
    begin: Ctl
    wire [6:1] nctl = ~ctl;
    cy_psoc3_statusi #(.cy_force_order(`TRUE), .cy_md_select(7'h0), .cy_int_mask(7'h0)) Mon2
    (
        /* input         */ .clock(op_clk),
        /* input [06:00] */ .status({ctl_flt1, nctl}),
        /* output        */ .interrupt(ctl_flt2)
    );
    end 
    endgenerate
    
    /* Generation of ctl[6:1] fault condition */
    wire ctlf = (NumCtlInputs > 3'd0) ? ctl_flt2 : 1'b0; /* Assign 0s to optimize if ctl[x] not used */
    
    /***************************************************************************
    *  Control Register to drive fault, warn, sys_stable, sys_up and sys_dn                    
    ***************************************************************************/
    wire [7:0] sys_status;
    /* Control register bit locations */
    localparam [2:0] SEQUENCER_FAULT      = 3'd0;
    localparam [2:0] SEQUENCER_WARN       = 3'd1;
    localparam [2:0] SEQUENCER_SYS_STABLE = 3'd2;
    localparam [2:0] SEQUENCER_SYS_UP     = 3'd3;
    localparam [2:0] SEQUENCER_SYS_DN     = 3'd4;
    
    cy_psoc3_control #(.cy_force_order(1)) SystemStsReg
    (
        /* output [07:00] */  .control(sys_status)
    );    
    
    /* Move sys_stable signal from bus_clk domain to component clock domain
    *  in order to meet timing in Rapid Fault Response block.
    */
    assign fault       = sys_status[SEQUENCER_FAULT];
    assign warn        = sys_status[SEQUENCER_WARN];
    assign sys_stable  = sys_status[SEQUENCER_SYS_STABLE];
    assign sys_up      = sys_status[SEQUENCER_SYS_UP];
    assign sys_dn      = sys_status[SEQUENCER_SYS_DN];
    
    /***************************************************************************
    *  Control Register to drive sts[6:1] outputs                    
    ***************************************************************************/
    wire [7:0] stsout;
    generate 
    if(NumStsOutputs > 0)
    begin: STS
        cy_psoc3_control #(.cy_force_order(1)) GeneralStsReg
        (
            /* output [07:00] */  .control(stsout)
        );     
    end
    endgenerate
    
    assign sts[6:1] = stsout[5:0];
    
    /***************************************************************************
    *           Status Registers for Converter Pgood Monitoring                    
    ***************************************************************************/
    /* Converter power good (pgood1 through pgood8) Status Register */
    generate
    if(NumConverters > 0)
    begin: PG1
        cy_psoc3_status #(.cy_force_order(`TRUE), .cy_md_select(8'h00)) PgoodReg
        (
            /* input         */ .clock(op_clk),
            /* input [07:00] */ .status(pgood_bus[8:1])
        ); 
    end
    endgenerate
    
    /* Converter power good (pgood9 through pgood16) Status Register */
    generate
    if(NumConverters > 8)
    begin: PG2
        cy_psoc3_status #(.cy_force_order(`TRUE), .cy_md_select(8'h00)) PgoodReg
        (
            /* input         */ .clock(op_clk),
            /* input [07:00] */ .status(pgood_bus[16:9])
        ); 
    end
    endgenerate
    
    /* Converter power good (pgood17 through pgood24) Status Register */
    generate
    if(NumConverters > 16)
    begin: PG3
        cy_psoc3_status #(.cy_force_order(`TRUE), .cy_md_select(8'h00)) PgoodReg
        (
            /* input         */ .clock(op_clk),
            /* input [07:00] */ .status(pgood_bus[24:17])
        ); 
    end
    endgenerate
    
    /* Converter power good (pgood25 through pgood32) Status Register */
    generate
    if(NumConverters > 24)
    begin: PG4
        cy_psoc3_status #(.cy_force_order(`TRUE), .cy_md_select(8'h00)) PgoodReg
        (
            /* input         */ .clock(op_clk),
            /* input [07:00] */ .status(pgood_bus[32:25])
        ); 
    end
    endgenerate
    
    /***************************************************************************
    *           Control Registers for Converter Enable Driving                    
    ***************************************************************************/
    wire [32:1] fwen; /* firmware driven enable */  
    /* Converter enable (en1 through en8) Control Register */
    generate
    if(NumConverters > 0)
    begin: EN1
        cy_psoc3_control #(.cy_force_order(1)) EnReg
        (
            /* output [07:00] */  .control(fwen[8:1])
        ); 
    end
    endgenerate
    
    /* Converter enable (en9 through en16) Control Register */
    generate
    if(NumConverters > 8)
    begin: EN2
        cy_psoc3_control #(.cy_force_order(1)) EnReg
        (
            /* output [07:00] */  .control(fwen[16:9])
        ); 
    end
    endgenerate
    
    /* Converter enable (en17 through en25) Control Register */
    generate
    if(NumConverters > 16)
    begin: EN3
        cy_psoc3_control #(.cy_force_order(1)) EnReg
        (
            /* output [07:00] */  .control(fwen[24:17])
        );
    end
    endgenerate
    
    /* Converter enable (en9 through en16) Control Register */
    generate
    if(NumConverters > 24)
    begin: EN4
        cy_psoc3_control #(.cy_force_order(1)) EnReg
        (
            /* output [07:00] */  .control(fwen[32:25])
        ); 
    end
    endgenerate
    
    /***************************************************************************
    *               Fault interrupt generation logic
    ****************************************************************************
    * Fault interrupt generation logic requires 2 macrocells and one control 
    * register per 8 power converters. Control register is required to provide
    * knowledge for HW that converter's SW state machine is in the ON state.
    */
    /* Pipelining the logic for better performance and optimal PLD usage */
    reg pgf1; /* fault condition due to de-assertion of pgood1..4   */ 
    reg pgf2; /* fault condition due to de-assertion of pgood5..8   */
    reg pgf3; /* fault condition due to de-assertion of pgood9..12  */
    reg pgf4; /* fault condition due to de-assertion of pgood13..16 */
    reg pgf5; /* fault condition due to de-assertion of pgood17..20 */
    reg pgf6; /* fault condition due to de-assertion of pgood21..24 */
    reg pgf7; /* fault condition due to de-assertion of pgood25..28 */
    reg pgf8; /* fault condition due to de-assertion of pgood29..32 */
    
    wire [32:1] on;
    
    generate
    if(NumConverters > 0)
    begin: ON1
        cy_psoc3_control #(.cy_force_order(1)) OnReg
        (
            /* output [07:00] */  .control(on[8:1])
        ); 
    end
    endgenerate
    
    generate
    if(NumConverters > 8)
    begin: ON2
        cy_psoc3_control #(.cy_force_order(1)) OnReg
        (
            /* output [07:00] */  .control(on[16:9])
        ); 
    end
    endgenerate
    
    generate
    if(NumConverters > 16)
    begin: ON3
        cy_psoc3_control #(.cy_force_order(1)) OnReg
        (
            /* output [07:00] */  .control(on[24:17])
        ); 
    end
    endgenerate
    
    generate
    if(NumConverters > 24)
    begin: ON4
        cy_psoc3_control #(.cy_force_order(1)) OnReg
        (
            /* output [07:00] */  .control(on[32:25])
        ); 
    end
    endgenerate
    
    generate 
    if(NumConverters > 0)
    begin: PGF1
    always @(posedge op_clk)
        begin
            pgf1 <= ((~pgood_bus[4:1] & on[4:1]) != 4'h0);
        end
    end
    endgenerate
    generate 
    if(NumConverters > 4)
    begin: PGF2
        always @(posedge op_clk)
        begin
            pgf2 <= ((~pgood_bus[8:5] & on[8:5]) != 4'h0);
        end
    end
    endgenerate
    generate 
    if(NumConverters > 8)
    begin: PGF3
    always @(posedge op_clk)
        begin
            pgf3 <= ((~pgood_bus[12:9] & on[12:9]) != 4'h0);        
        end    
    end
    endgenerate
    generate 
    if(NumConverters > 12)
    begin: PGF4
        always @(posedge op_clk)
        begin
            pgf4 <= ((~pgood_bus[16:13] & on[16:13]) != 4'h0);
        end
    end
    endgenerate
    generate 
    if(NumConverters > 16)
    begin: PGF5
    always @(posedge op_clk)
        begin
            pgf5 <= ((~pgood_bus[20:17] & on[20:17]) != 4'h0);
        end    
    end
    endgenerate
    generate 
    if(NumConverters > 20)
    begin: PGF6
        always @(posedge op_clk)
        begin
            pgf6 <= ((~pgood_bus[24:21] & on[24:21]) != 4'h0);
        end
    end
    endgenerate
    generate 
    if(NumConverters > 24)
    begin: PGF7
    always @(posedge op_clk)
        begin
            pgf7 <= ((~pgood_bus[28:25] & on[28:25]) != 4'h0);
        end    
    end
    endgenerate
    generate 
    if(NumConverters > 28)
    begin: PGF8
        always @(posedge op_clk)
        begin
            pgf8 <= ((~pgood_bus[32:29] & on[32:29]) != 4'h0);
        end
    end
    endgenerate

    always @(posedge op_clk)
    begin
        fault_interrupt <=       (NumConverters <= 4)   ? pgf1 | ctlf :
        ((NumConverters >  4) && (NumConverters <= 8))  ? pgf1 | pgf2 | ctlf:
        ((NumConverters >  8) && (NumConverters <= 12)) ? pgf1 | pgf2 | pgf3 | ctlf:
        ((NumConverters > 12) && (NumConverters <= 16)) ? pgf1 | pgf2 | pgf3 | pgf4 | ctlf:
        ((NumConverters > 16) && (NumConverters <= 20)) ? pgf1 | pgf2 | pgf3 | pgf4 | pgf5 | ctlf :
        ((NumConverters > 20) && (NumConverters <= 24)) ? pgf1 | pgf2 | pgf3 | pgf4 | pgf5 | pgf6 | ctlf :
        ((NumConverters > 25) && (NumConverters <= 28)) ? pgf1 | pgf2 | pgf3 | pgf4 | pgf5 | pgf6 | pgf7 | ctlf :
                                                          pgf1 | pgf2 | pgf3 | pgf4 | pgf5 | pgf6 | pgf7 | pgf8 | ctlf;
    end
    
    /***************************************************************************
    *                    Rapid Fault Response Implementation
    ****************************************************************************
    * A simple macrocell based design allows hardware to bypass firmware control
    * of the en[x] outputs and take immediate action. The intention is to let 
    * firmware control the en[x] terminals during power up sequencing and during
    * intended power down sequencing. Hardware will control the en[x] terminals
    * when a pgood[x] fault condition occurs when the system is stable.
    */
    localparam SEQUENCER_FAULTRESP_STATE_IDLE     = 2'b00;
    localparam SEQUENCER_FAULTRESP_STATE_PWR_UP   = 2'b01;
    localparam SEQUENCER_FAULTRESP_STATE_ON       = 2'b11;
    localparam SEQUENCER_FAULTRESP_STATE_SHUTDOWN = 2'b10;
    
    wire [32:1] en_bus;
    genvar i;
    generate 
    for (i = 1; i <= NumConverters; i = i + 1)
    begin: FAULTRESP
        reg [1:0] state;
        always @(posedge op_clk)
        begin
            case (state)
                SEQUENCER_FAULTRESP_STATE_IDLE: 
                if(fwen[i])
                begin
                    state <= SEQUENCER_FAULTRESP_STATE_PWR_UP;
                end
                SEQUENCER_FAULTRESP_STATE_PWR_UP:
                if(~fwen[i])
                begin
                    state <= SEQUENCER_FAULTRESP_STATE_IDLE;
                end
                else
                begin
                    if(on[i])
                    begin
                        state <= SEQUENCER_FAULTRESP_STATE_ON;
                    end
                    else
                    begin
                        state <= SEQUENCER_FAULTRESP_STATE_PWR_UP;
                    end
                end    
                SEQUENCER_FAULTRESP_STATE_ON:
                if(~pgood_bus[i] | ~fwen[i])
                begin
                    state <= SEQUENCER_FAULTRESP_STATE_SHUTDOWN;
                end
                SEQUENCER_FAULTRESP_STATE_SHUTDOWN:
                if(~fwen[i] & ~pgood_bus[i])
                begin
                    state <= SEQUENCER_FAULTRESP_STATE_IDLE;
                end    
                default:
                    state <= SEQUENCER_FAULTRESP_STATE_IDLE;
            endcase
        end
        assign en_bus[i] = state[0];
    end
    endgenerate
    
    /* Output assignment */
    assign en1 = en_bus[1];         
    assign en2 = en_bus[2];         
    assign en3 = en_bus[3];
    assign en4 = en_bus[4];
    assign en5 = en_bus[5];
    assign en6 = en_bus[6];
    assign en7 = en_bus[7];
    assign en8 = en_bus[8];
    assign en9 = en_bus[9];
    assign en10 = en_bus[10];
    assign en11 = en_bus[11];
    assign en12 = en_bus[12];
    assign en13 = en_bus[13];
    assign en14 = en_bus[14];
    assign en15 = en_bus[15];
    assign en16 = en_bus[16];
    assign en17 = en_bus[17];
    assign en18 = en_bus[18];
    assign en19 = en_bus[19];
    assign en20 = en_bus[20];
    assign en21 = en_bus[21];
    assign en22 = en_bus[22];
    assign en23 = en_bus[23];
    assign en24 = en_bus[24];
    assign en25 = en_bus[25];
    assign en26 = en_bus[26];
    assign en27 = en_bus[27];
    assign en28 = en_bus[28];
    assign en29 = en_bus[29];
    assign en30 = en_bus[30];
    assign en31 = en_bus[31];
    assign en32 = en_bus[32];
    
endmodule

`endif /* bSPDIF_Tx_v1_10_V_ALREADY_INCLUDED */
