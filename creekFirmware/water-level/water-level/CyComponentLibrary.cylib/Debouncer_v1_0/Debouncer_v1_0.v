/*******************************************************************************
* File Name: Debouncer_v1_0.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* The debouncer component is a hardware method of eliminating the oscillations
* that occur on transitions of an input digital signal from most types of 
* switches. This file describes the component functionality in Verilog.
*
********************************************************************************
* I*O Signals:
********************************************************************************
*   Name        Direction       Description
*   q           output          Filtered output signal
*   either      output          Either edge detected
*   neg         output          Negative edge detected
*   pos         output          Positive edge detected
*   clock       input           Sampling rate
*   d           input           Input signal
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation. All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"

`ifdef Debouncer_v1_0_V_ALREADY_INCLUDED
`else
`define Debouncer_v1_0_V_ALREADY_INCLUDED

module Debouncer_v1_0 (
    q,          /* Filtered output  */
    either,     /* Either edge      */
    neg,        /* Negative edge    */
    pos,        /* Positive edge    */
    clock,      /* Sampling rate    */
    d           /* Input signal     */
);

    /***************************************************************************
    *             Parameters                                                
    ***************************************************************************/
    parameter [5:0] SignalWidth = 6'd1;
    parameter EitherEdgeDetect  = 1'b1;
    parameter NegEdgeDetect     = 1'b1;
    parameter PosEdgeDetect     = 1'b1;
    
    /***************************************************************************
    *            Interface Definition                                                
    ***************************************************************************/
    output wire[SignalWidth-1:0] q;
    output  reg[SignalWidth-1:0] pos;
    output  reg[SignalWidth-1:0] neg;
    output  reg[SignalWidth-1:0] either;
    input  wire[SignalWidth-1:0] d;
    input  wire clock;

    /***************************************************************************
    *         Instantiation of udb_clock_enable  
    ****************************************************************************
    * The udb_clock_enable primitive component allows to support clock enable 
    * mechanism and specify the intended synchronization behaviour for the clock 
    * result (operational clock).
    */
    wire op_clk;    /* operational clock */
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(op_clk)
    ); 

    /* N-bit debouncer is implemented as N 1-bit debouncers */
    genvar i; 
    generate
    for(i = 0; i < SignalWidth; i = i + 1)
    begin: DEBOUNCER
        reg [1:0] d_sync;
        always @(posedge op_clk)
        begin
            d_sync[0] <= d[i];
            d_sync[1] <= d_sync[0];
        end

        /* Output Generation */
        assign q[i] = d_sync[0];
        always @(posedge op_clk)
        begin
            pos[i]    <= (PosEdgeDetect)    ?  d_sync[0] & ~d_sync[1] : 1'b0;
            neg[i]    <= (NegEdgeDetect)    ? ~d_sync[0] &  d_sync[1] : 1'b0;
            either[i] <= (EitherEdgeDetect) ?  d_sync[0] ^  d_sync[1] : 1'b0;
        end
    end
    endgenerate
        
endmodule

`endif /* Debouncer_v1_0_V_ALREADY_INCLUDED */
