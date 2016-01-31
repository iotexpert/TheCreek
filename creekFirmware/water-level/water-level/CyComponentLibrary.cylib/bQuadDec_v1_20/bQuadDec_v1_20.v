/*******************************************************************************
* File Name: bQuadDec_v1_20.v
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
*******************************************************************************/

`include "cypress.v"
`ifdef QD_v1_20_V_ALREADY_INCLUDED
`else
`define QD_v1_20_V_ALREADY_INCLUDED

module bQuadDec_v1_20 (
    input     wire    clock,
    input     wire    quad_A,
    input     wire    quad_B,
    input     wire    index,
    input     wire    overflow,
    input     wire    underflow,
    output    wire    count,
    output    reg     dir,
    output    reg     error,
    output    wire    reset,
    output    wire    interrupt
    );    
    
	localparam QD_SM_STATE_AB_00 = 4'd0;
	localparam QD_SM_STATE_BA_00 = 4'd1;
	localparam QD_SM_STATE_AB_10 = 4'd2;
	localparam QD_SM_STATE_AB_11 = 4'd3;
	localparam QD_SM_STATE_AB_01 = 4'd4;
	localparam QD_SM_STATE_BA_10 = 4'd5;
	localparam QD_SM_STATE_BA_11 = 4'd6;
	localparam QD_SM_STATE_BA_01 = 4'd7;
	localparam QD_SM_STATE_ERROR = 4'd8;
	
	localparam COUNTER_RESOLUTION_1X = 3'd1;
	localparam COUNTER_RESOLUTION_2X = 3'd2;
	localparam COUNTER_RESOLUTION_4X = 3'd4;
	
	localparam DIRECTION_CLOCKWISE     = 1'b0;
	localparam DIRECTION_ANTICLOCKWISE = 1'b1;
	
    parameter CounterResolution     = 0;
    parameter UsingGlitchFiltering  = 0;
    parameter UsingIndexInput       = 0;
    
	reg [3:0] state;  
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
    wire count_2x;
    wire count_2x_trig;
    wire reset_delayed;
    
    generate    
        if(UsingGlitchFiltering)
        begin        
            cy_dff DelayA1 (			    
                /* output */    .q(quad_A_delayed[0]),
                /* input  */    .d(quad_A), 
                /* input  */    .clk(clock)               
            );
            cy_dff DelayA2 (
                /* output */    .q(quad_A_delayed[1]),
                /* input  */    .d(quad_A_delayed[0]),  
                /* input  */    .clk(clock)               
            );
            cy_dff DelayA3 (
                /* output */    .q(quad_A_delayed[2]),
                /* input  */    .d(quad_A_delayed[1]),  
                /* input  */    .clk(clock)               
            );
            
            assign A_j = quad_A_delayed[0] & quad_A_delayed[1] & quad_A_delayed[2];
            assign A_k = (~quad_A_delayed[0]) & (~quad_A_delayed[1]) & (~quad_A_delayed[2]);
            
            QD_jk_ff_v1_0 QuadATriger (
                /* output */    .q(quad_A_filt),
                /* input  */    .clk(clock),
                /* input  */    .k(A_k),
                /* input  */    .j(A_j)                                 
            );
            
            cy_dff DelayB1 (
                /* output */    .q(quad_B_delayed[0]),
                /* input  */    .d(quad_B), 
                /* input  */    .clk(clock)               
            );
            cy_dff DelayB2 (
                /* output */    .q(quad_B_delayed[1]),
                /* input  */    .d(quad_B_delayed[0]),  
                /* input  */    .clk(clock)               
            );
            cy_dff DelayB3 (
                /* output */    .q(quad_B_delayed[2]),
                /* input  */    .d(quad_B_delayed[1]),  
                /* input  */    .clk(clock)               
            );
            
            assign B_j = quad_B_delayed[0] & quad_B_delayed[1] & quad_B_delayed[2];
            assign B_k = (~quad_B_delayed[0]) & (~quad_B_delayed[1]) & (~quad_B_delayed[2]);
            
            QD_jk_ff_v1_0 QuadBTriger (
                /* output */    .q(quad_B_filt),
                /* input  */    .clk(clock),
                /* input  */    .k(B_k),
                /* input  */    .j(B_j)
            );                        
        end
        else 
        begin      	
            cy_dff DelayQuadA (
                /* output */    .q(quad_A_filt),
                /* input  */    .d(quad_A),  
                /* input  */    .clk(clock)               
            );
            cy_dff DelayQuadB (
                /* output */    .q(quad_B_filt),
                /* input  */    .d(quad_B),  
                /* input  */    .clk(clock)               
            );
        end
        
        if (UsingIndexInput)
        begin             
            assign reset = ~(quad_A_filt | quad_B_filt | index_filt);		
            if (CounterResolution != COUNTER_RESOLUTION_4X)
            begin              
                cy_dff ResetTriger (
                    /* output */    .q(reset_delayed),
                    /* input  */    .d(reset),  
                    /* input  */    .clk(clock)               
                );                
            end
            
            if(UsingGlitchFiltering)
            begin    
                cy_dff DelayIndex1 (
                    /* output */    .q(index_delayed[0]),
                    /* input  */    .d(index), 
                    /* input  */    .clk(clock)               
                );
                cy_dff DelayIndex2 (
                    /* output */    .q(index_delayed[1]),
                    /* input  */    .d(index_delayed[0]),  
                    /* input  */    .clk(clock)               
                );
                cy_dff DelayIndex3 (
                    /* output */    .q(index_delayed[2]),
                    /* input  */    .d(index_delayed[1]),  
                    /* input  */    .clk(clock)               
                );
            
                assign index_j = index_delayed[0] & index_delayed[1] & index_delayed[2];
                assign index_k = (~index_delayed[0]) & (~index_delayed[1]) & (~index_delayed[2]);
    
                QD_jk_ff_v1_0 IndexTriger (
                    /* output */    .q(index_filt),
                    /* input  */    .clk(clock),
                    /* input  */    .k(index_k),
                    /* input  */    .j(index_j)
                );            
            end
            else
            begin                            					
                cy_dff DelayIndex (
                    /* output */    .q(index_filt),
                    /* input  */    .d(index),  
                    /* input  */    .clk(clock)               
                );
            end                            
        end
        else
        begin            
            assign reset = 1'b0;
            assign reset_delayed = 1'b0;
        end        
		
        if (CounterResolution == COUNTER_RESOLUTION_2X)
        begin       
            assign count = ~(quad_A_filt ^ quad_B_filt) ^ reset ^ reset_delayed;                           /* counter resolution = 2x */
        end
        else if (CounterResolution == COUNTER_RESOLUTION_4X)
        begin            
            assign count_2x = quad_A_filt ^ quad_B_filt;        				          /* counter resolution =  4x */
            cy_dff DelayCount2x (
                /* output */    .q(count_2x_trig),
                /* input  */    .d(count_2x),  
                /* input  */    .clk(clock)               
            );
            assign count = ~(count_2x_trig ^ count_2x); 
        end
        else
        begin             
            assign count = ~quad_B_filt ^ reset ^ reset_delayed;                                  		/* counter resolution = 1x */
        end        
    endgenerate                     
     
    /* State register block */      
	always @(posedge clock)
	begin
		case (state)		
			QD_SM_STATE_AB_00:		
				if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b0))
				begin
					state <= QD_SM_STATE_AB_10;
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b0))	
				begin
					state <= QD_SM_STATE_AB_00;		
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b1))
				begin
					state <= QD_SM_STATE_BA_10;
				end
				else 
				begin
					state <= QD_SM_STATE_ERROR;
				end
                
			QD_SM_STATE_BA_00:		
				if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b0))
				begin
					state <= QD_SM_STATE_AB_10;
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b0))	
				begin
					state <= QD_SM_STATE_BA_00;		
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b1))
				begin
					state <= QD_SM_STATE_BA_10;
				end
				else 
				begin
					state <= QD_SM_STATE_ERROR;
				end
			
			QD_SM_STATE_AB_10:		
				if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b1))
				begin
					state <= QD_SM_STATE_AB_11;	
				end
				else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b0))
				begin
					state <= QD_SM_STATE_AB_10;						
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b0))
				begin
					state <= QD_SM_STATE_AB_00;
				end
				else 
				begin
					state <= QD_SM_STATE_ERROR;
				end
			
			QD_SM_STATE_AB_11:	
				if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b1))
				begin
					state <= QD_SM_STATE_AB_01;					
				end
				else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b1))
				begin
					state <= QD_SM_STATE_AB_11;					
				end
				else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b0))	
				begin
					state <= QD_SM_STATE_BA_01;
				end
				else 
				begin
					state <= QD_SM_STATE_ERROR;
				end
							
			QD_SM_STATE_AB_01:	
				if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b0))
				begin
					state <= QD_SM_STATE_AB_00;					
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b1))
				begin
					state <= QD_SM_STATE_AB_01;					
				end
				else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b1))	
				begin
					state <= QD_SM_STATE_AB_11;
				end
				else 
				begin
					state <= QD_SM_STATE_ERROR;
				end
				
			QD_SM_STATE_BA_10:		
				if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b1))	
				begin
					state <= QD_SM_STATE_BA_11;						
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b1))						
				begin
					state <= QD_SM_STATE_BA_10;					
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b0))
				begin
					state <= QD_SM_STATE_BA_00;
				end
				else
				begin
					state <= QD_SM_STATE_ERROR;
				end
				
			QD_SM_STATE_BA_11:		
				if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b0))	
				begin
					state <= QD_SM_STATE_BA_01;						
				end
				else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b1))						
				begin
					state <= QD_SM_STATE_BA_11;						
				end
				else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b1))
				begin
					state <= QD_SM_STATE_AB_01;
				end
				else
				begin
					state <= QD_SM_STATE_ERROR;	
				end
				
			QD_SM_STATE_BA_01:		
				if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b0))	
				begin
					state <= QD_SM_STATE_BA_00;		
				end
				else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b0))						
				begin
					state <= QD_SM_STATE_BA_01;						
				end
				else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b1))						
				begin
					state <= QD_SM_STATE_AB_11;
				end
				else 
				begin
					state <= QD_SM_STATE_ERROR;		
				end
		
			QD_SM_STATE_ERROR:		
				if (dir == DIRECTION_CLOCKWISE)
				begin
					if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b0))
					begin
						state <= QD_SM_STATE_AB_00;
					end
					else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b1))
					begin
						state <= QD_SM_STATE_AB_01;
					end
					else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b0))
					begin
						state <= QD_SM_STATE_AB_10;
					end
					else 
					begin
						state <= QD_SM_STATE_AB_11;	
					end
				end
				else
				begin
					if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b0))
					begin
						state <= QD_SM_STATE_BA_00;
					end
					else if ((quad_A_filt == 1'b0) && (quad_B_filt == 1'b1))
					begin
						state <= QD_SM_STATE_BA_01;
					end
					else if ((quad_A_filt == 1'b1) && (quad_B_filt == 1'b0))
					begin
						state <= QD_SM_STATE_BA_10;
					end
					else 
					begin
						state <= QD_SM_STATE_BA_11;
					end
				end		
		default: state <= QD_SM_STATE_AB_00;
		endcase
	end	  
	
	/* Output logic */	
	always @(state)
	begin
		if (state == QD_SM_STATE_AB_00)
		begin
			dir <= DIRECTION_CLOCKWISE;
			error <= 1'b0;
		end				
		else if (state == QD_SM_STATE_BA_00)
		begin
			dir <= DIRECTION_ANTICLOCKWISE;
			error <= 1'b0;
		end		
		else if (state == QD_SM_STATE_AB_10)
		begin
			dir <= DIRECTION_CLOCKWISE;
			error <= 1'b0; 
		end		
		else if (state == QD_SM_STATE_AB_11)
		begin
			dir <= DIRECTION_CLOCKWISE;
			error <= 1'b0;			
		end			
		else if (state == QD_SM_STATE_AB_01)
		begin
			dir <= DIRECTION_CLOCKWISE;
			error <= 1'b0;			
		end
		else if (state == QD_SM_STATE_BA_10)
		begin
			dir <= DIRECTION_ANTICLOCKWISE;
			error <= 1'b0;			
		end		
		else if (state == QD_SM_STATE_BA_11)
		begin
			dir <= DIRECTION_ANTICLOCKWISE;
			error <= 1'b0;			
		end	
		else if (state == QD_SM_STATE_BA_01)
		begin
			dir <= DIRECTION_ANTICLOCKWISE;
			error <= 1'b0;			
		end		
		else if (state == QD_SM_STATE_ERROR)
		begin
			error <= 1'b1;        	
		end
	end 
          
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
    
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h7F), 
            .cy_int_mask(7'h7F)) 
    Stsreg(
    /* input          */  .clock(clock),
    /* input  [06:00] */  .status(status),
    /* output         */  .interrupt(interrupt)
    );

endmodule

`endif      /* QD_v1_20_V_ALREADY_INCLUDED */

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