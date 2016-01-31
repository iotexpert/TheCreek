/*******************************************************************************
* File Name: bQuadDec_v2_10.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides a top level model of the Quadrature Decoder component
*  defining and all of the necessary interconnect.
*
* Note:
*
*********************************************************************************
*                 Control and Status Register definitions
******************************************************************************** 
*
* Statusi Register Definition
*  +=======+---------+------+------+------+-----+-----+---------+--------+
*  |  Bit  |    7    |  6   |  5   |  4   |  3  |  2  |    1    |   0    |
*  +=======+---------+------+------+------+-----+-----+---------+--------+
*  | Desc  |interrupt|       unused       |error|reset|underflow|overflow|
*  +=======+---------+------+------+------+-----+-----+---------+--------+
*
*  overflow    => 0 = overflow event has not occured 
*                 1 = overflow event has occured
*
*  underflow   => 0 = underflow event has not occured 
*                 1 = underflow event has occured
*
*  reset       => 0 = reset event has not occured 
*                 1 = reset event has occured
*
*  error       => 0 = error event has not occured 
*                 1 = error event has occured
*
********************************************************************************
*                 IO Signals:
********************************************************************************
* IO SIGNALS:
*   clock               input        System clock
*   quad_A              input        Input signal A
*   quad_B              input        Input signal B
*   index               input        Input signal Index
*   overflow            input        Input Overflow condition
*   underflow           input        Input Underflow condition
*   enable              output       Enable signal
*   dir                 output       Count direction
*   reset               output       Reset to Counter 
*   interrupt           output       Interrupt output
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/

`include "cypress.v"
`ifdef QD_v2_10_V_ALREADY_INCLUDED
`else
`define QD_v2_10_V_ALREADY_INCLUDED

module bQuadDec_v2_10 (
    input     wire    clock,
    input     wire    quad_A,
    input     wire    quad_B,
    input     wire    index,
    input     wire    overflow,
    input     wire    underflow,
    output    reg     enable,
    output    reg     dir,
    output    wire    reset,
    output    wire    interrupt
    );    
    
    localparam QD_SM_STATE_AB_00 = 4'b0000;    
    localparam QD_SM_STATE_AB_01 = 4'b0001;
    localparam QD_SM_STATE_AB_10 = 4'b0010;
    localparam QD_SM_STATE_AB_11 = 4'b0011;
    localparam QD_SM_STATE_RESET = 5'b0100;
    localparam QD_SM_STATE_ERROR = 5'b1000;    
    
    localparam QD_AB_00 = 2'b00;    
    localparam QD_AB_01 = 2'b01;
    localparam QD_AB_10 = 2'b10;
    localparam QD_AB_11 = 2'b11;
    
    localparam COUNTER_RESOLUTION_1X = 3'd1;
    localparam COUNTER_RESOLUTION_2X = 3'd2;
    localparam COUNTER_RESOLUTION_4X = 3'd4;
    
    localparam DIRECTION_CLOCKWISE     = 1'b0;
    localparam DIRECTION_ANTICLOCKWISE = 1'b1;
    
    localparam INDEX_INPUT_USED          = 1'd1;
    localparam INDEX_INPUT_NOT_USED      = 1'd0;
    localparam GLITCH_FILTERING_USED     = 1'd1;
    localparam GLITCH_FILTERING_NOT_USED = 1'd0;
    parameter CounterResolution     = 8'd0;
    parameter UsingGlitchFiltering  = GLITCH_FILTERING_NOT_USED;
    parameter UsingIndexInput       = INDEX_INPUT_NOT_USED;
    
    reg [3:0] state;  
    wire error;
    wire [2:0] quad_A_delayed;
    wire [2:0] quad_B_delayed;
    wire [2:0] index_delayed;
    wire quad_A_filt;
    wire quad_B_filt;
    wire index_filt;
    wire A_j;
    wire A_k;
    wire B_j;
    wire B_k;    
    wire index_j;
    wire index_k;
    wire sync_clock;
    
    /**************************************************************************/
    /*      Clock Enable for Control Register and other clocked elements      */
    /**************************************************************************/
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    CtrlClkEn (        
        .clock_in(clock),
        .enable(1'b1),
        .clock_out(sync_clock)
    );
    
    generate    
        if(UsingGlitchFiltering)      /* Selection three samples of each input signal for Glitch filtering */
        begin        
            cy_dff DelayA1 (                
                /* output */    .q(quad_A_delayed[0]),       /* Subsynchronous input quad_A signal*/
                /* input  */    .d(quad_A),                  /* Input signal A */
                /* input  */    .clk(sync_clock)             /* System clock */
            );
            cy_dff DelayA2 (
                /* output */    .q(quad_A_delayed[1]),       /* Delayed Subsynchronous input quad_A signal */
                /* input  */    .d(quad_A_delayed[0]),       /* Subsynchronous input quad_A signal */
                /* input  */    .clk(sync_clock)             /* System clock */
            );
            cy_dff DelayA3 (
                /* output */    .q(quad_A_delayed[2]),       /* Twice Delayed Subsynchronous input quad_A signal */
                /* input  */    .d(quad_A_delayed[1]),       /* Delayed Subsynchronous input quad_A signal */
                /* input  */    .clk(sync_clock)             /* System clock */
            );
            
            assign A_j = quad_A_delayed[0] & quad_A_delayed[1] & quad_A_delayed[2];
            assign A_k = (~quad_A_delayed[0]) & (~quad_A_delayed[1]) & (~quad_A_delayed[2]);
            
            QD_jk_ff_v1_0 QuadATriger (
                /* output */    .q(quad_A_filt),             /* Filtered input quad_A signal */
                /* input  */    .clk(sync_clock),            /* System clock */
                /* input  */    .k(A_k),                     /* Wired AND of inversion of three contiguous samples 
                                                                input quad_A signal */
                /* input  */    .j(A_j)                      /* Wired AND of three contiguous samples input quad_A 
                                                                signal */              
            );
            
            cy_dff DelayB1 (
                /* output */    .q(quad_B_delayed[0]),       /* Subsynchronous input quad_B signal */
                /* input  */    .d(quad_B),                  /* Input signal B */
                /* input  */    .clk(sync_clock)             /* System clock */
            );
            cy_dff DelayB2 (
                /* output */    .q(quad_B_delayed[1]),       /* Delayed Subsynchronous input quad_B signal */
                /* input  */    .d(quad_B_delayed[0]),       /* Subsynchronous input quad_B signal */
                /* input  */    .clk(sync_clock)             /* System clock */
            );
            cy_dff DelayB3 (
                /* output */    .q(quad_B_delayed[2]),       /* Twice Delayed Subsynchronous input quad_B signal */
                /* input  */    .d(quad_B_delayed[1]),       /* Delayed Subsynchronous input quad_B signal */
                /* input  */    .clk(sync_clock)             /* System clock */
            );
            
            assign B_j = quad_B_delayed[0] & quad_B_delayed[1] & quad_B_delayed[2];
            assign B_k = (~quad_B_delayed[0]) & (~quad_B_delayed[1]) & (~quad_B_delayed[2]);
            
            QD_jk_ff_v1_0 QuadBTriger (
                /* output */    .q(quad_B_filt),             /* Filtered input quad_B signal */
                /* input  */    .clk(sync_clock),            /* System clock */
                /* input  */    .k(B_k),                     /* Wired AND of inversion of three contiguous samples
                                                                input quad_B signal */
                /* input  */    .j(B_j)                      /* Wired AND of three contiguous samples input quad_B 
                                                                signal */
            );                        
        end
        else 
        begin          
            cy_dff DelayQuadA (
                /* output */    .q(quad_A_filt),             /* Filtered input quad_A signal */
                /* input  */    .d(quad_A),                  /* Input signal A */
                /* input  */    .clk(sync_clock)             /* System clock */
            );
            cy_dff DelayQuadB (
                /* output */    .q(quad_B_filt),             /* Filtered input quad_B signal */
                /* input  */    .d(quad_B),                  /* Input signal B */
                /* input  */    .clk(sync_clock)             /* System clock */
            );
        end
        
        if (UsingIndexInput)
        begin                                                         
            if(UsingGlitchFiltering)
            begin    
                cy_dff DelayIndex1 (
                    /* output */    .q(index_delayed[0]),    /* Subsynchronous input Index signal */
                    /* input  */    .d(index),               /* Input signal Index */
                    /* input  */    .clk(sync_clock)         /* System clock */
                );
                cy_dff DelayIndex2 (
                    /* output */    .q(index_delayed[1]),    /* Delayed Subsynchronous input Index signal */
                    /* input  */    .d(index_delayed[0]),    /* Subsynchronous input Index signal */
                    /* input  */    .clk(sync_clock)         /* System clock */
                );
                cy_dff DelayIndex3 (
                    /* output */    .q(index_delayed[2]),    /* Twice Delayed Subsynchronous input Index signal */
                    /* input  */    .d(index_delayed[1]),    /* Delayed Subsynchronous input Index signal */
                    /* input  */    .clk(sync_clock)         /* System clock */
                );
            
                assign index_j = index_delayed[0] & index_delayed[1] & index_delayed[2];
                assign index_k = (~index_delayed[0]) & (~index_delayed[1]) & (~index_delayed[2]);
    
                QD_jk_ff_v1_0 IndexTriger (
                    /* output */    .q(index_filt),          /* Filtered input Index signal */
                    /* input  */    .clk(sync_clock),        /* System clock */
                    /* input  */    .k(index_k),             /* Wired AND of inversion of three contiguous samples 
                                                                input Index signal */
                    /* input  */    .j(index_j)              /* Wired AND of three contiguous samples input Index 
                                                                signal */
                );            
            end
            else
            begin                                                
                cy_dff DelayIndex (
                    /* output */    .q(index_filt),          /* Filtered input Index signal */
                    /* input  */    .d(index),               /* Input signal Index */
                    /* input  */    .clk(sync_clock)         /* System clock */
                );
            end                            
        end
        else
        begin            
            assign index_filt = index;
        end        
        
        assign reset = state[2];
        assign error = state[3];
        
        /* State machine implementation for 1x Counter resolution */
        if (CounterResolution == COUNTER_RESOLUTION_1X)
        begin                  
            always @(posedge sync_clock)
            begin
                case (state)        
                    QD_SM_STATE_AB_00:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end 
                            default: 
                                if (index_filt == 1'b1)    
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    enable <= 1'b0;
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                    enable <= 1'b0;
                                end                                
                        endcase
                        
                    QD_SM_STATE_AB_01:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:    
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    dir <= 1'b0;
                                    enable <= 1'b0;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    dir <= 1'b1;
                                    enable <= 1'b0;
                                end 
                            default: 
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    enable <= 1'b0;
                                end
                        endcase
                        
                    QD_SM_STATE_AB_10:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    dir <= 1'b1;
                                    enable <= 1'b0;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end                            
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    dir <= 1'b0;
                                    enable <= 1'b0;
                                end 
                            default: 
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    enable <= 1'b0;
                                end
                        endcase
                        
                    QD_SM_STATE_AB_11:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b0;
                                    enable <= 1'b0;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b1;
                                    enable <= 1'b0;
                                end
                            default:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    enable <= 1'b0;
                                end
                        endcase
                 
                    QD_SM_STATE_RESET:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end 
                            default:
                                if (index_filt == 1'b1)    
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    enable <= 1'b0;
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                end
                        endcase
                        
                    QD_SM_STATE_ERROR:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                if (index_filt == 1'b1)    
                                begin
                                    state <= QD_SM_STATE_AB_00;                                   
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                end 
                            default:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                end
                        endcase
                    default: 
                        begin
                            state <= QD_SM_STATE_RESET;
                            dir <= 1'b0;
                            enable <= 1'b0;
                        end
                endcase
            end 
        end
        
        /* State machine implementation for 2x Counter resolution */
        else if(CounterResolution == COUNTER_RESOLUTION_2X)
        begin
            always @(posedge sync_clock)
            begin
                case (state)        
                    QD_SM_STATE_AB_00:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end 
                            default: 
                                if (index_filt == 1'b1)
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    enable <= 1'b0;
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                    enable <= 1'b0;
                                end                                
                        endcase
                        
                    QD_SM_STATE_AB_01:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:  
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    dir <= 1'b0;
                                    enable <= 1'b0;
                                end                                                           
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end 
                            default: 
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    enable <= 1'b0;
                                end
                        endcase
                        
                    QD_SM_STATE_AB_10:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:  
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    dir <= 1'b1;
                                    enable <= 1'b0;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end                            
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end 
                            default: 
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    enable <= 1'b0;
                                end
                        endcase
                        
                    QD_SM_STATE_AB_11:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b0;
                                    enable <= 1'b0;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b1;
                                    enable <= 1'b0;
                                end
                            default:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    enable <= 1'b0;
                                end
                        endcase
                 
                    QD_SM_STATE_RESET:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end 
                            default:
                                if (index_filt == 1'b1)    
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    enable <= 1'b0;
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                end
                        endcase
                        
                    QD_SM_STATE_ERROR:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                if (index_filt == 1'b1)    
                                begin
                                    state <= QD_SM_STATE_AB_00;                                   
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                end 
                            default:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                end
                        endcase
                    default:
                        begin
                            state <= QD_SM_STATE_RESET;
                            dir <= 1'b0;
                            enable <= 1'b0;
                        end
                endcase
            end 
        end
        
        /* State machine implementation for 4x Counter resolution */
        else if(CounterResolution == COUNTER_RESOLUTION_4X)
        begin
            always @(posedge sync_clock)
            begin
                case (state)        
                    QD_SM_STATE_AB_00:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end 
                            default: 
                                if (index_filt == 1'b1)
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    enable <= 1'b0;
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                    enable <= 1'b0;
                                end                                
                        endcase
                        
                    QD_SM_STATE_AB_01:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end 
                            default: 
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    enable <= 1'b0;
                                end
                        endcase
                        
                    QD_SM_STATE_AB_10:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end                            
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end 
                            default: 
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    enable <= 1'b0;
                                end
                        endcase
                        
                    QD_SM_STATE_AB_11:  
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end
                            default:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                    enable <= 1'b0;
                                end
                        endcase
                 
                    QD_SM_STATE_RESET:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                    dir <= 1'b1;
                                    enable <= 1'b1;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                    dir <= 1'b0;
                                    enable <= 1'b1;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                    enable <= 1'b0;
                                end 
                            default:
                                if (index_filt == 1'b1)    
                                begin
                                    state <= QD_SM_STATE_AB_00;
                                    enable <= 1'b0;
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                end
                        endcase
                        
                    QD_SM_STATE_ERROR:
                        case ({quad_A_filt, quad_B_filt})
                            QD_AB_00:
                                if (index_filt == 1'b1)    
                                begin
                                    state <= QD_SM_STATE_AB_00;                                   
                                end
                                else    
                                begin
                                    state <= QD_SM_STATE_RESET;
                                end
                            QD_AB_01:
                                begin
                                    state <= QD_SM_STATE_AB_01;
                                end
                            QD_AB_10:
                                begin
                                    state <= QD_SM_STATE_AB_10;
                                end
                            QD_AB_11:
                                begin
                                    state <= QD_SM_STATE_AB_11;
                                end 
                            default:
                                begin
                                    state <= QD_SM_STATE_ERROR;
                                end
                        endcase
                    default:
                        begin
                            state <= QD_SM_STATE_RESET;
                            dir <= 1'b0;
                            enable <= 1'b0;
                        end                    
                endcase
            end           
        end
    endgenerate                     
       
    /**************************************************************************/
    /* Status Register Implementation                                         */
    /**************************************************************************/
    
    /* Status Register Bits (Bits 4-6 are unused */
    
    localparam QD_STS_OVERFLOW  = 3'h0;         /* Counter overflow                                   */
    localparam QD_STS_UNDERFLOW = 3'h1;         /* Counter underflow                                  */
    localparam QD_STS_RESET     = 3'h2;         /* Counter reset due to index, if index input is used */
    localparam QD_STS_INVALID   = 3'h3;         /* Invalid A, B inputs state transition               */
    localparam QD_STS_UNUSED1   = 3'h4;
    localparam QD_STS_UNUSED2   = 3'h5; 
    localparam QD_STS_UNUSED3   = 3'h6; 
        
    wire    [6:0]    status;                    /* Status Register Input */

    assign status[QD_STS_OVERFLOW]   = overflow;
    assign status[QD_STS_UNDERFLOW]  = underflow;
    assign status[QD_STS_RESET]      = reset;
    assign status[QD_STS_INVALID]    = error;
    assign status[QD_STS_UNUSED1]    = 1'b0;
    assign status[QD_STS_UNUSED2]    = 1'b0;
    assign status[QD_STS_UNUSED3]    = 1'b0;    
    
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h0F), 
            .cy_int_mask(7'h0F))
    Stsreg(
    /* input          */  .clock(sync_clock),             /* System clock */
    /* input  [06:00] */  .status(status),                /* Inputs of Statusi Register */
    /* output         */  .interrupt(interrupt)           /* Interrupt output */
    );

endmodule

`endif    /* QD_v2_10_V_ALREADY_INCLUDED */

`ifdef QD_jk_ff_v1_0_V_ALREADY_INCLUDED
`else
`define QD_jk_ff_v1_0_V_ALREADY_INCLUDED
    /* Implementation of JK flip-flop */
    module QD_jk_ff_v1_0 (
        output reg q,
        input wire j,
        input wire k,
        input wire clk
    );
        always @ (posedge clk)
        begin
            case ({j, k})
                2'b00 : q = q;
                2'b01 : q = 1'b0;
                2'b10 : q = 1'b1;
                2'b11 : q = ~q;
            endcase
        end
    endmodule
`endif      /* QD_jk_ff_v1_0_V_ALREADY_INCLUDED */
