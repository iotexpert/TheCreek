/*******************************************************************************
* File Name: GlitchFilter_v2_0.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* The glitch filter component removes unwanted pulses from a digital input 
* signal. 
* This file describes the component functionality in Verilog.
*
********************************************************************************
* Data Path register definitions
********************************************************************************
* GlitchFilter: FiltCount
* DESCRIPTION: FiltCount is used to count pulse width.
* REGISTER USAGE:
* F0 => not used
* F1 => not used
* D0 => filter pulse width in clocks
* D1 => not used
* A0 => used to count for D0
* A1 => not used
*
********************************************************************************
* I*O Signals:
********************************************************************************
*   Name        Direction       Description
*   q           output          Filtered output signal
*   reset       input           Synchronous reset
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

`ifdef GlitchFilter_v2_0_V_ALREADY_INCLUDED
`else
`define GlitchFilter_v2_0_V_ALREADY_INCLUDED

module GlitchFilter_v2_0 (
	q,      /* Filtered output   */
	clock,  /* Sample clock      */
	d,      /* Signal to filter  */ 
	reset   /* Synchronous reset */
);

    /***************************************************************************
    *             Parameters                                                
    ***************************************************************************/
    parameter [4:0] SignalWidth  = 5'd1;
    parameter [8:0] GlitchLength = 9'd3; /* The max value is 256 */
    parameter [1:0] BypassFilter = 2'd0;
    
    /***************************************************************************
    *            Interface Definition                                                
    ***************************************************************************/
    output wire[SignalWidth-1:0] q;
    input  wire[SignalWidth-1:0] d;
    input  wire reset;
    input  wire clock;

    /* Bypass Filter Configurations */
    localparam [1:0] GLITCH_FILTER_BYPASS_NONE = 2'd0;
    localparam [1:0] GLITCH_FILTER_BYPASS_ONE  = 2'd1;
    localparam [1:0] GLITCH_FILTER_BYPASS_ZERO = 2'd2;
	    
    /***************************************************************************
    * Filter counter resulution. Counter is implemented in PLD in case of 
    * glitch filter length is less or equal to 8 samlpes.
    ***************************************************************************/
    localparam [2:0] CountResolution = (GlitchLength <= 9'd2) ? 3'd1 :
                                       (GlitchLength <= 9'd4) ? 3'd2 : 3'd3;
    
    localparam [8:0] NumSamples = GlitchLength - 1;
    
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
    
    genvar i;   
    generate
    for(i = 0; i < SignalWidth; i = i + 1)
    begin
        if(GlitchLength == 1)
        /* Special case. Doesn't require flip-flops chained into shift register */
        begin
            reg sample;
            reg last_state;            
            wire or_term  = d[i] | sample;
            wire and_term = d[i] & sample;
            
            always @(posedge op_clk)
            begin
                if(reset)
                begin
                    sample     <= 1'b0;
                    last_state <= 1'b0;
                end
                else
                begin
                    sample <= d[i];
                    case(BypassFilter)
                        GLITCH_FILTER_BYPASS_ONE:
                        begin
                            last_state <= (d[i]) ? d[i] : last_state & or_term | and_term;
                        end
                        GLITCH_FILTER_BYPASS_ZERO:
                        begin
                            last_state <= (d[i]) ? last_state & or_term | and_term : d[i];
                        end
                        default: /* BYPASS_NONE */
                           last_state <= last_state & or_term | and_term;
                    endcase
                end
            end
            /* Assign filtered value to the output */
            assign q[i] = last_state;
        end
        else if(GlitchLength <= 8)
        begin
            reg [NumSamples:0] samples;
            reg last_state;
            
            wire or_term  = |{samples, d[i]};
            wire and_term = &{samples, d[i]};
            
            always @(posedge op_clk)
            begin
                if(reset)
                begin
                    samples <= 0;
                    last_state <= 1'b0;
                end
                else
                begin
                    samples[NumSamples:0] <= {samples[NumSamples-1:0], d[i]};                    
                    case(BypassFilter)
                        GLITCH_FILTER_BYPASS_ONE:
                        begin
                            last_state <= (d[i]) ? d[i] : last_state & or_term | and_term;
                        end
                        GLITCH_FILTER_BYPASS_ZERO:
                        begin
                            last_state <= (d[i]) ? last_state & or_term | and_term : d[i];
                        end
                        default: /* BYPASS_NONE */
                           last_state <= last_state & or_term | and_term;
                    endcase
                end
            end           
            /* Assign filtered value to the output */
            assign q[i] = last_state;
        end
    end
    endgenerate
    
    generate 
    if(GlitchLength > 8)
    begin
        /***********************************************************************
        * Glitch filter includes a counter and simple 2-states state machine. 
        * The state machine state register is the output of the filter. The 
        * state machine makes state transitions only if the input changes and 
        * then remains unchanged for the specified number of samples.
        ***********************************************************************/
        localparam GLITCH_FILTER_STATE_ZERO = 1'b0;
        localparam GLITCH_FILTER_STATE_ONE  = 1'b1;
        
        reg [SignalWidth-1:0] state;
        wire[SignalWidth-1:0] counter_done;
        
        /* Datapath instance is configured as counter with sync reset */
        localparam  CounterCfg = {
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0:   reset=0, d=0, state=0 => Reload A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1:   reset=0, d=0, state=1 => Dec A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2:   reset=0, d=1, state=0 => Dec A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3:   reset=0, d=1, state=1 => Reload A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4:   reset=1, d=0, state=0 => Reload A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5:   reset=1, d=0, state=1 => Reload A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6:   reset=1, d=1, state=0 => Reload A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7:   reset=1, d=1, state=1 => Reload A0*/
            8'hFF, 8'h00,  /*CFG9:    */
            8'hFF, 8'hFF,  /*CFG11-10:    */
            `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12:    */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14:    */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:    */
        };
       
        for(i = 0; i < SignalWidth; i = i + 1)
        begin
            always @(posedge op_clk)
            begin
                if(reset)
                begin
                    state[i] <= 1'b0;
                end
                else
                begin
                    case(state[i])
                        GLITCH_FILTER_STATE_ZERO:
                            if(BypassFilter == GLITCH_FILTER_BYPASS_ONE)
                            begin
                                if(d[i])
                                begin
                                    state <= GLITCH_FILTER_STATE_ONE;
                                end
                            end
                            else
                            begin
                                if(d[i] & counter_done[i])
                                begin
                                    state[i] <= GLITCH_FILTER_STATE_ONE;
                                end
                            end
                        GLITCH_FILTER_STATE_ONE:
                            if(BypassFilter == GLITCH_FILTER_BYPASS_ZERO)
                            begin
                                if(~d[i])
                                begin
                                    state <= GLITCH_FILTER_STATE_ZERO;
                                end
                            end
                            else
                            begin
                                if(~d[i] & counter_done[i])
                                begin
                                    state[i] <= GLITCH_FILTER_STATE_ZERO;
                                end
                            end
                        default:
                            state[i] <= GLITCH_FILTER_STATE_ZERO;
                    endcase
                end
            end 
            /* Assign filtered value to the output */
            assign q[i] = state[i];
		end

        if(SignalWidth > 0)
        begin: Counter0
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[0], state[0]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[0]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
        end
        
        if(SignalWidth > 1)
        begin: Counter1
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[1], state[1]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[1]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end

        if(SignalWidth > 2)
        begin: Counter2
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[2], state[2]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[2]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end

        if(SignalWidth > 3)
        begin: Counter3
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[3], state[3]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[3]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 4)
        begin: Counter4
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[4], state[4]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[4]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 5)
        begin: Counter5
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[5], state[5]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[5]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 6)
        begin: Counter6
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[6], state[6]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[6]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 7)
        begin: Counter7
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[7], state[7]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[7]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
        end
        
        if(SignalWidth > 8)
        begin: Counter8
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[8], state[8]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[8]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 9)
        begin: Counter9
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[9], state[9]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[9]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 10)
        begin: Counter10
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[10], state[10]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[10]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 11)
        begin: Counter11
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[11], state[11]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[11]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 12)
        begin: Counter12
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[12], state[12]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[12]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 13)
        begin: Counter13
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[13], state[13]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[13]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 14)
        begin: Counter14
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[14], state[14]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[14]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 15)
        begin: Counter15
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[15], state[15]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[15]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 16)
        begin: Counter16
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[16], state[16]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[16]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 17)
        begin: Counter17
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[17], state[17]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[17]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 18)
        begin: Counter18
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[18], state[18]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[18]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 19)
        begin: Counter19
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[19], state[19]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[19]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 20)
        begin: Counter20
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[20], state[20]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[20]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 21)
        begin: Counter21
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[21], state[21]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[21]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 22)
        begin: Counter22
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[22], state[22]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[22]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
        if(SignalWidth > 23)
        begin: Counter23
            cy_psoc3_dp8 #(.d0_init_a(NumSamples), .cy_dpconfig_a(CounterCfg)
    		) DP(
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(op_clk),
                /*  input   [02:00]         */  .cs_addr({reset, d[23], state[23]}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(counter_done[23]),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(),
                /*  output                  */  .cmsb(),
                /*  output                  */  .so(),
                /*  output                  */  .f0_bus_stat(),
                /*  output                  */  .f0_blk_stat(),
                /*  output                  */  .f1_bus_stat(),
                /*  output                  */  .f1_blk_stat()
            );
    	end
        
    end    
	endgenerate

endmodule

`endif /* GlitchFilter_v2_0_V_ALREADY_INCLUDED */
