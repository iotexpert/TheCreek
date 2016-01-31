/*******************************************************************************
* File Name: bQuadDec_v1_10.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides a top level model of the Quadrature Decoder component
*  defining and all of the necessary interconnect.
*
* Note:
*
********************************************************************************
*                 IO Signals:
********************************************************************************
* IO SIGNALS:
*   start_reset         input        System reset
*   clock               input        System clock
*   quad_A              input        Input signal A
*   quad_B              input        Input signal B
*   index               input        Input signal index
*   overflow            input        Input Overflow condition
*   underflow           input        Input Underflow condition
*   count               output       Count signal
*   dir                 output       Count direction
*   error               output       Error condition
*   reset               output       Reset to Counter 
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




`include "cypress.v"
`ifdef QD_v1_10_V_ALREADY_INCLUDED
`else
`define QD_v1_10_V_ALREADY_INCLUDED

module bQuadDec_v1_10 (
    input     wire    clock,
    input     wire    bus_clock,
    input     wire    index,
    input     wire    quad_A,
    input     wire    quad_B,
    input     wire    overflow,
    input     wire    underflow,
    input     wire    start_reset,
    output    wire    count,
    output    wire    dir,
    output    wire    error,
    output    wire    reset,
    output    wire    interrupt   
    );    
    
    parameter CounterResolution     = 0;
    parameter UsingGlitchFiltering  = 0;
    parameter UsingIndexInput       = 0;
    
    wire stateA;
    wire stateB;    
    wire [2:0] quad_A_delayed;
    wire [2:0] quad_B_delayed;
    wire [2:0] index_delayed;
    wire quad_A_filt;
    wire quad_B_filt;
    wire index_filt;
    wire final_reset;
    wire A_j;
    wire A_k;
    wire B_j;
    wire B_k;    
    wire index_j;
    wire index_k;
    wire count_2x;
    wire count_2x_trig;
    wire pre_dir;
    
    generate    
        if(UsingGlitchFiltering)
        begin        
            d_ff_rst DelayA1 (
                .q(quad_A_delayed[0]),
                .d(quad_A), 
                .clk(clock),
                .rst(start_reset)                
            );
            
            d_ff_rst DelayA2 (
                .q(quad_A_delayed[1]),
                .d(quad_A_delayed[0]), 
                .clk(clock),
                .rst(start_reset)                
            );
            
            d_ff_rst DelayA3 (
                .q(quad_A_delayed[2]),
                .d(quad_A_delayed[1]), 
                .clk(clock),
                .rst(start_reset)                
            );
            
            assign A_j = quad_A_delayed[0] & quad_A_delayed[1] & quad_A_delayed[2];
            assign A_k = (~quad_A_delayed[0]) & (~quad_A_delayed[1]) & (~quad_A_delayed[2]);
            
            jk_ff QuadATriger (
                .q(quad_A_filt),
                .rst(start_reset),
                .clk(clock),
                .k(A_k),
                .j(A_j)                                 
            );
            
            d_ff_rst DelayB1 (
                .q(quad_B_delayed[0]),
                .d(quad_B), 
                .clk(clock),
                .rst(start_reset)                
            );
            
            d_ff_rst DelayB2 (
                .q(quad_B_delayed[1]),
                .d(quad_B_delayed[0]), 
                .clk(clock),
                .rst(start_reset)                
            );
            
            d_ff_rst DelayB3 (
                .q(quad_B_delayed[2]),
                .d(quad_B_delayed[1]), 
                .clk(clock),
                .rst(start_reset)                
            );                
            
            assign B_j = quad_B_delayed[0] & quad_B_delayed[1] & quad_B_delayed[2];
            assign B_k = (~quad_B_delayed[0]) & (~quad_B_delayed[1]) & (~quad_B_delayed[2]);
            
            jk_ff QuadBTriger (
                .q(quad_B_filt),
                .rst(start_reset),
                .clk(clock),
                .k(B_k),
                .j(B_j)
            );                        
        end
        else 
        begin    
            d_ff_rst DelayQuadA (
                .q(quad_A_filt),
                .d(quad_A),
                .clk(clock),
                .rst(start_reset)
            );
            d_ff_rst DelayQuadB (
                .q(quad_B_filt),
                .d(quad_B),
                .clk(clock),
                .rst(start_reset)
            );                                
        end
        
        if (UsingIndexInput)
        begin
            assign reset = ~(quad_A_filt | quad_B_filt | index_filt);
            assign final_reset = reset | start_reset;    
            
            if(UsingGlitchFiltering)
            begin    
                d_ff_rst DelayIndex1 (
                .q(index_delayed[0]),
                .d(index), 
                .clk(clock),
                .rst(start_reset)                
                );
                
                d_ff_rst DelayIndex2 (
                    .q(index_delayed[1]),
                    .d(index_delayed[0]), 
                    .clk(clock),
                    .rst(start_reset)
                );
                
                d_ff_rst DelayIndex3 (
                    .q(index_delayed[2]),
                    .d(index_delayed[1]), 
                    .clk(clock),
                    .rst(start_reset)
                );
                
                assign index_j = index_delayed[0] & index_delayed[1] & index_delayed[2];
                assign index_k = (~index_delayed[0]) & (~index_delayed[1]) & (~index_delayed[2]);
    
                jk_ff IndexTriger (
                    .q(index_filt),
                    .rst(start_reset),
                    .clk(clock),
                    .k(index_k),
                    .j(index_j)
                );            
            end
            else
            begin            
                assign index_filt = index;
            end                            
        end
        else
        begin            
            assign final_reset = start_reset;
            assign reset = 1'b0;
        end
        
        if (CounterResolution == 8'd2)
        begin            
            assign count = stateA ^ stateB;            /* counter resolution = 2x */
        end
        else if (CounterResolution == 8'd4)
        begin            
            assign count_2x = stateA ^ stateB;        /* counter resolution =  4x */
            d_ff_rst DelayCount2x (
                .q(count_2x_trig),
                .d(count_2x), 
                .clk(clock),
                .rst(final_reset)                
            );                
            assign count = count_2x_trig ^ count_2x; 
        end
        else
        begin            
            assign count = quad_A_filt;                    /* counter resolution = 1x */
        end        
    endgenerate
    
    d_ff_rst StateATriger (
        .q(stateA),
        .d(quad_A_filt), 
        .clk(clock),
        .rst(start_reset)                
    );
        
    d_ff_rst StateBTriger (
        .q(stateB),
        .d(quad_B_filt), 
        .clk(clock),
        .rst(start_reset)                
    );
    
    d_ff_rst DirTriger (
        .q(dir),
        .d(quad_B_filt), 
        .clk(quad_A_filt),
        .rst(start_reset)                
    ); 

    
    
    
    assign error = ( (~quad_A_filt) & quad_B_filt & stateA & (~stateB)) | 
                    ((~quad_A_filt) & (~quad_B_filt) & stateA & stateB) | 
                    (quad_A_filt & (~quad_B_filt) & (~stateA) & stateB) | 
                    (quad_A_filt & quad_B_filt & (~stateA) & (~stateB) );    
    
    wire d_overflow;
    wire int_overflow;
    wire d_underflow;
    wire int_underflow;
    wire d_reset;
    wire int_reset;
    wire d_error;
    wire int_error;
    
    d_ff_rst D_Ov(
        .q(d_overflow),
        .d(overflow), 
        .clk(bus_clock),
        .rst(1'b0)                
    ); 

    d_ff_rst D_Un(
        .q(d_underflow),
        .d(underflow), 
        .clk(bus_clock),
        .rst(1'b0)                
    ); 

    d_ff_rst D_Rst(
        .q(d_reset),
        .d(reset), 
        .clk(bus_clock),
        .rst(1'b0)                
    ); 

    d_ff_rst D_Err(
        .q(d_error),
        .d(error), 
        .clk(bus_clock),
        .rst(1'b0)                
    ); 

    assign int_overflow = overflow & (~d_overflow);
    assign int_underflow = underflow & (~d_underflow);
    assign int_reset = reset & (~d_reset);
    assign int_error = error & (~d_error);
    
    /**************************************************************************/
    /* Status Register Implementation                                         */
    /**************************************************************************/
    
    /* Status Register Bits (Bits 4-6 are unused */
    
    localparam QD_STS_OVERFLOW  = 8'h0;         /* Counter overflow                                   */
    localparam QD_STS_UNDERFLOW = 8'h1;         /* Counter underflow                                  */
    localparam QD_STS_RESET     = 8'h2;         /* Counter reset due to index, if index input is used */
    localparam QD_STS_INVALID   = 8'h3;         /* Invalid A, B inputs state transition               */
    localparam QD_STS_UNUSED1   = 8'h4;
    localparam QD_STS_UNUSED2   = 8'h5; 
    localparam QD_STS_UNUSED3   = 8'h6; 
        
    wire    [6:0]    status;                    /* Status Register Input */

    assign status[QD_STS_OVERFLOW]   = int_overflow;
    assign status[QD_STS_UNDERFLOW]  = int_underflow;
    assign status[QD_STS_RESET]      = int_reset;
    assign status[QD_STS_INVALID]    = int_error;
    assign status[QD_STS_UNUSED1]    = 1'b0;
    assign status[QD_STS_UNUSED2]    = 1'b0;
    assign status[QD_STS_UNUSED3]    = 1'b0;    
    
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h7F), 
            .cy_int_mask(7'h7F)) 
    Stsreg(
    /* input          */  .clock(bus_clock),
    /* input  [06:00] */  .status(status),
    /* output         */  .interrupt(interrupt)
    );

//`#end` -- edit above this line, do not edit this line
endmodule

/* Implementation of D flip-flop */
module d_ff_rst (
    output reg q,
    input wire d,
    input wire clk,
    input wire rst   
);
    always @(posedge clk or posedge rst)
    begin
        if(rst)
        begin
            q = 1'b0;
        end
        else 
        begin
            q = d;
        end
    end
endmodule

/* Implementation of JK flip-flop */
module jk_ff (
    output reg q,
    input wire j,
    input wire k,
    input wire clk,
    input wire rst    
);
    always @ (posedge clk or posedge rst)
    begin
        if(rst == 1'b1)
        begin
            q = 1'b0;
        end
        else
        begin
            case ({j, k})
                2'b00 : q = q;
                2'b01 : q = 1'b0;
                2'b10 : q = 1'b1;
                2'b11 : q = ~q;
            endcase
        end
    end
endmodule

`endif      /* QD_v1_10_V_ALREADY_INCLUDED */

