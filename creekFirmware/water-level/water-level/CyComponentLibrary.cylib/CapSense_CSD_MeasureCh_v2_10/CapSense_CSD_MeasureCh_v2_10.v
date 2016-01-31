/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef CapSense_CSD_MeasureCh_v2_10_V_ALREADY_INCLUDED
`else
`define CapSense_CSD_MeasureCh_v2_10_V_ALREADY_INCLUDED

module CapSense_CSD_MeasureCh_v2_10
(
    input    wire        clock,
    input    wire        reset,
    input    wire        start,
    input    wire        pulse,
    input    wire        cmp_in,
    input    wire        enable,
    output   wire        ioff,
    output   wire        interrupt
);

    /**************************************************************************/
    /* Parameters                                                             */
    /**************************************************************************/
      /* Resourse selection options */
    localparam CAPSNS_MEASURE_IMPLEMENTATION_FF    = 1'd0;
    localparam CAPSNS_MEASURE_IMPLEMENTATION_UDB   = 1'd1;
    
    parameter ImplementationType = CAPSNS_MEASURE_IMPLEMENTATION_UDB;
    
    
    /* Idac options parameters */
    localparam CAPSNS_IDAC_OPTIONS_NONE = 2'd0;
    localparam CAPSNS_IDAC_OPTIONS_SRC  = 2'd1;
    localparam CAPSNS_IDAC_OPTIONS_SINC = 2'd2;
    
    parameter IdacOptions = CAPSNS_IDAC_OPTIONS_SRC; 

   /* Window generator states */
    localparam CAPSNS_MEASURE_CH_IDLE   = 4'd0;  /* Idle */
    localparam CAPSNS_MEASURE_CH_LOAD   = 4'd1;  /* Load */
    localparam CAPSNS_MEASURE_CH_S1     = 4'd2;  /* S1 */
    localparam CAPSNS_MEASURE_CH_COUNT  = 4'd4;  /* Count */
    localparam CAPSNS_MEASURE_CH_END    = 4'd8;  /* End*/
    
    localparam dpconfig_cntr =
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3: */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4: */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5: */
        `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7: */
         8'h00, 8'h00,  /*CFG9: */
         8'h00, 8'h00,  /*CFG11-10: */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
         1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14: */
         10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*CFG17-16: */
   };

    
    /* Internal signals */
    wire int;
    wire op_clock;  /* Clock to operate the component */
    wire cnt_enable;
    wire load_enable;
    wire win_enable;
    
    reg [3:0] wndState;
    reg pre_cmp_in_reg;
    reg cmp_in_reg;
    reg int_reg;
    
   
    /*ImplementationType == CAPSNS_MEASURE_IMPLEMENTATION_FF*/
    wire Wndw_enable;
    wire Cntr_enable;
    
    /*ImplementationType == CAPSNS_MEASURE_IMPLEMENTATION_UDB*/
    wire [2:0] cs_addr_win;
    wire [2:0] cs_addr_cnt;
    wire zw0,zw1;    /* Window zerro detect signal */
    wire zc0,zc1;    /* Counter zerro detect signal */

    reg win_enable_reg;
    reg cnt_enable_reg;
   
    /**************************************************************************/
    /* Hierarchy - instantiating another module                               */
    /**************************************************************************/
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */ .clock_in(clock),
        /* input  */ .enable(enable),
        /* output */ .clock_out(op_clock)
    );
    
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync2
    (
        /* input  */ .clock_in(clock),
        /* input  */ .enable(pulse),
        /* output */ .clock_out(trig_clock)
    );

    
    generate
        if(ImplementationType == CAPSNS_MEASURE_IMPLEMENTATION_FF)
        begin: FF
            cy_psoc3_timer_v1_0 Window
            (
                /* input */     .clock(clock),  /* Use clock because there is not clock_enable componnent for FF*/
                /* input */     .kill(1'b0),
                /* input */     .enable(win_enable_reg),
                /* input */     .capture(1'b0),
                /* input */     .timer_reset(reset),
                /* output */    .tc(int),
                /* output */    .compare(),
                /* output */    .interrupt()
            );
            
              cy_psoc3_timer_v1_0 Counter
            (
                /* input */     .clock(clock),  /* Use clock because there is not clock_enable componnent for FF*/
                /* input */     .kill(1'b0),
                /* input */     .enable(cnt_enable_reg),
                /* input */     .capture(1'b0),
                /* input */     .timer_reset(reset),
                /* output */    .tc(),
                /* output */    .compare(),
                /* output */    .interrupt()
            );
        end
        else
        begin: UDB
            cy_psoc3_dp8 #(.a0_init_a(8'hFF), .a1_init_a(8'hFF), .d0_init_a(8'hFF),
            .cy_dpconfig_a(dpconfig_cntr)) 
            Window(
                /*  input                   */  .clk(op_clock),
                /*  input   [02:00]         */  .cs_addr(cs_addr_win),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(zw0),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(zw1),
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
            
            cy_psoc3_dp8 #(.a0_init_a(8'hFF), .a1_init_a(8'hFF), .d0_init_a(8'hFF),
            .cy_dpconfig_a(dpconfig_cntr))
            Counter(
                /*  input                   */  .clk(op_clock),
                /*  input   [02:00]         */  .cs_addr(cs_addr_cnt),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(zc0),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(zc1),
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
    endgenerate
    
    
    /**************************************************************************/
    /* Synchronous procedures                                                 */
    /**************************************************************************/
    
    always @(posedge op_clock)
    begin
        if (reset)
            wndState <= CAPSNS_MEASURE_CH_IDLE;
        else
            case(wndState)
                CAPSNS_MEASURE_CH_IDLE:     if (start)
                                                                        wndState <= CAPSNS_MEASURE_CH_LOAD;
                                            else                        wndState <= CAPSNS_MEASURE_CH_IDLE;
                CAPSNS_MEASURE_CH_LOAD:                                 wndState <= CAPSNS_MEASURE_CH_S1;
                CAPSNS_MEASURE_CH_S1:       if (pulse)                  wndState <= CAPSNS_MEASURE_CH_COUNT;
                                            else                        wndState <= CAPSNS_MEASURE_CH_S1;
                CAPSNS_MEASURE_CH_COUNT:    if (int)                    wndState <= CAPSNS_MEASURE_CH_END;
                                            else                        wndState <= CAPSNS_MEASURE_CH_S1;
                CAPSNS_MEASURE_CH_END:      if (start==0)               wndState <= CAPSNS_MEASURE_CH_IDLE;
                                            else                        wndState <= CAPSNS_MEASURE_CH_END;
                default:                                                wndState <= CAPSNS_MEASURE_CH_IDLE;
            endcase
    end

    generate
        if(ImplementationType == CAPSNS_MEASURE_IMPLEMENTATION_FF)
        begin
            always @(posedge op_clock)win_enable_reg = Wndw_enable;
            always @(posedge op_clock)cnt_enable_reg = Cntr_enable;
        end
    endgenerate

    
    /* regiser cmp_in signal */
    generate
        if(IdacOptions == CAPSNS_IDAC_OPTIONS_SRC)
        begin
            always @(posedge trig_clock)
            begin
                pre_cmp_in_reg <= cmp_in;
                cmp_in_reg <= pre_cmp_in_reg;
            end
        end
        else
        begin
            always @(posedge trig_clock)
            begin
                pre_cmp_in_reg <= ~cmp_in; 
                cmp_in_reg <= pre_cmp_in_reg;
            end
        end
    endgenerate

    /**************************************************************************/
    /* Combinatinal procedures                                                */
    /**************************************************************************/

    generate
        if(ImplementationType == CAPSNS_MEASURE_IMPLEMENTATION_FF)
        begin
            assign Wndw_enable=((win_enable && ~int)||load_enable);
            assign Cntr_enable=((cnt_enable && ~int)||load_enable);
        end
        else
        begin
            assign cs_addr_win = (load_enable)
            ? (3'b001)
                : ((win_enable)
                    ? ({1'b1,zw0,zw1}) : (3'b000));
            
            assign cs_addr_cnt = (load_enable)
            ? (3'b001)
                : ((cnt_enable)
                    ? ({1'b1,zc0,zc1}) : (3'b000));

            assign int = (zw0 && zw1);
        end
    endgenerate
    
    
    assign load_enable = wndState[0];
    assign win_enable = wndState[2] & ~int;
    
    assign interrupt =  wndState[3];
    assign ioff = cmp_in_reg;
    assign cnt_enable = ~cmp_in_reg & win_enable;
    
endmodule

`endif /* CapSense_CSD_MeasureCh_v2_10_0_V ALREADY_INCLUDED */

