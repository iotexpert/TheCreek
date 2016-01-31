/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef CapSense_CSD_ClockGen_v2_0_V_ALREADY_INCLUDED
`else
`define CapSense_CSD_ClockGen_v2_0_V_ALREADY_INCLUDED

module CapSense_CSD_ClockGen_v2_0 
(
    input    wire   clock,
    input    wire   reset,
    input    wire   enable,
    output   wire   dpulse,
    output   wire   ppulse,
    output   wire   start0,
    output   wire   start1,
    output   wire   mesrst,
    output   wire   shield,
    output   wire   [23:0] d
);     

    /**************************************************************************/
    /* Parameters                                                             */
    /**************************************************************************/
    /* PRS Parameters */
    /**************************************************************************/

    localparam CAPSNS_CLK_GEN_PRS_OPTIONS_NONE       = 2'd0;
    localparam CAPSNS_CLK_GEN_PRS_OPTIONS_8BITS      = 2'd1;
    localparam CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS     = 2'd2;
    localparam CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS_TDM = 2'd3;
    parameter PrsOptions = CAPSNS_CLK_GEN_PRS_OPTIONS_NONE;

   /* Device Family and Silicon Revision definitions */
   /* PSoC3 ES2 or earlier */
   localparam PSOC3_ES2 = ((`CYDEV_CHIP_MEMBER_USED   == `CYDEV_CHIP_MEMBER_3A) && 
                          (`CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_3A_ES2));
   /* PSoC5 ES1 or earlier */
   localparam PSOC5_ES1 = ((`CYDEV_CHIP_MEMBER_USED   == `CYDEV_CHIP_MEMBER_5A) && 
                          (`CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_5A_ES1));
    
    
    /* State Machine States */
    localparam PRS_STATE_CALC_LOWER = 2'd0;  /* Calculate Lower Half */
    localparam PRS_STATE_SAVE_LOWER = 2'd1;  /* Save Lower Half */
    localparam PRS_STATE_CALC_UPPER = 2'd2;  /* Calculate Upper Half */
    localparam PRS_STATE_SAVE_UPPER = 2'd3;  /* Save Upper Half */
    
   
    localparam [2:0] dpPOVal = (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_NONE)  ? (3'b000): (3'b111);

    localparam [2:0] dpMsbVal = (3'b111);
    
    localparam dpconfig0 =
    {
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Calculate Lower Half */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Save Lower Half*/
        `CS_ALU_OP__XOR, `CS_SRCA_A1, `CS_SRCB_D1,
        `CS_SHFT_OP___SL, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Calculate Upper Half */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Save Upper Half*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG5 Comment:*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG7 Comment:*/
          8'hFF, 8'h00, /*SC_REG4    Comment: */
          8'hFF, 8'hFF, /*SC_REG5    Comment: */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_CHAIN,
        `SC_CI_A_ROUTE, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_ROUTE, /*SC_REG6 Comment: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
        1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*SC_REG7 Comment: */
         10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_ENBL,
        `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    };
    
    
    /* Others parameters */
    localparam CAPSNS_CLK_GEN_PRESCALER_NONE = 2'd0;
    localparam CAPSNS_CLK_GEN_PRESCALER_UDB  = 2'd1;
    localparam CAPSNS_CLK_GEN_PRESCALER_FF   = 2'd2;
    parameter PrescalerOptions = CAPSNS_CLK_GEN_PRESCALER_UDB;
    
    localparam CAPSNS_CLK_GEN_IDAC_OPTIONS_NONE     = 2'd0;
    localparam CAPSNS_CLK_GEN_IDAC_OPTIONS_SOURCE   = 2'd1;
    localparam CAPSNS_CLK_GEN_IDAC_OPTIONS_SINK     = 2'd2;
    parameter IdacOptions = CAPSNS_CLK_GEN_IDAC_OPTIONS_SOURCE;
    
    localparam CAPSNS_CLK_GEN_IDLE  = 3'd0;
    localparam CAPSNS_CLK_GEN_S1    = 3'd1;
    localparam CAPSNS_CLK_GEN_S2    = 3'd2;
    localparam CAPSNS_CLK_GEN_S3    = 3'd4;
    
    /* State Machine States */
    localparam CAPSNS_MEASURE_CH_IDLE   = 3'd0;
    localparam CAPSNS_MEASURE_CH_S1     = 3'd1;
    localparam CAPSNS_MEASURE_CH_S2     = 3'd2;
    
    reg [2:0] cstate;
    reg ppulse_dly;
    
    wire [7:0] control; /* Control Register Output */
    
    wire tmp_ppulse;
    wire tmp_dpulse;
    wire tmp_win1en;
    wire tmp_win2en;
    wire tmp_start0;
    wire tmp_start1;
    wire reset1;
    wire reset2;
    wire tmp_pclk;
    wire work_en;

    wire   syncen;
    wire   mesen;
    wire   ch0en;
    wire   ch1en;
    
    wire z0;
    wire [2:0] cs_addr;
    wire op_clock; /* clock to operate component */
    wire prescaler;
    
    /* PRS */
    wire so;
    wire bitstream;
    
    wire ci;
    wire si;
    wire cmsb;
    wire si_a;
    wire so_a;
    wire [7:0] sc_out_a;
    wire [7:0] sc_out;
    wire [2:0] prs_cs_addr;
        
    wire save_so;
    wire save;
    wire dcfg;
    wire so_reg1;
    
    reg so_reg;
    reg ci_temp;
    reg sc_temp;
    reg [1:0] state;
    
    reg syncen_reg;
    reg tmp_ppulse_dly;
    wire clock_detect;
    reg clock_detect_reg;
    
    wire ppulse_less;
    wire ppulse_equal;

    reg resetx, resetx_reg;
    wire resetx_all;
    
    /**************************************************************************/
    /* Hierarchy - instantiating another module                               */
    /**************************************************************************/
    
    /* udb_clock_enable instantiation */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync1
    (
        /* input  */    .clock_in(clock), 
        /* input  */    .enable(enable),
        /* output */    .clock_out(op_clock) 
    );
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync2
    (
        /* input  */    .clock_in(clock), 
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(clk_ctrl) 
    );

    /* PRS Clock */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkPrs_TDM
    (
        /* input  */    .clock_in(clock), 
        /* input  */    .enable(clock_detect_reg),
        /* output */    .clock_out(clk_TDM) 
    );

    /* control register instantiation */
    generate
        if (PSOC3_ES2 || PSOC5_ES1)
        begin: AsyncCtrl
            cy_psoc3_control #(.cy_force_order(1))
            CtrlReg (
                /* output 07:00] */.control(control)
            );
        end
        else
        begin: SyncCtrl
        
            cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFF))
            CtrlReg (
                /* input          */ .clock(clk_ctrl),
                /* output [07:00] */ .control(control)
            );
        end
    endgenerate

    /* Counter7 Instantiation */
    cy_psoc3_count7 #(.cy_route_ld(0), .cy_route_en(0), .cy_period(7'd31)) ScanSpeed
    (
        /* input        */ .clock(op_clock),
        /* input        */ .reset(reset1),
        /* input        */ .load(1'b0),
        /* input        */ .enable(1'b1), /* TODO: Need think about this*/
        /* output [6:0] */ .count(),
        /* output       */ .tc(tmp_dpulse)
    );

    
    generate
        if(PrescalerOptions == CAPSNS_CLK_GEN_PRESCALER_FF)
        begin: FF
            cy_psoc3_timer_v1_0 Prescaler
            (
                /* input */     .clock(clock), /* Use clock because there is not clock_enable componnent for FF*/
                /* input */     .kill(1'b0),
                /* input */     .enable(1'b1),
                /* input */     .capture(1'b0),
                /* input */     .timer_reset(reset1),
                /* output */    .tc(),
                /* output */    .compare(tmp_ppulse),
                /* output */    .interrupt()
            );    
        end
        else
        begin: UDB    
            cy_psoc3_dp8 #(.a0_init_a(8'hFF),
            .cy_dpconfig_a(
            {
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:COUNT*/
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:RESET/LOAD*/
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG2 Comment:LOAD*/
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG3 Comment:RESET*/
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG4 Comment:*/
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG5 Comment:*/
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG6 Comment:*/
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG7 Comment:*/
                  8'hFF, 8'h00, /*SC_REG4    Comment: */
                  8'hFF, 8'hFF, /*SC_REG5    Comment: */
                `SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
                `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
                `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
                `SC_SI_A_ROUTE, /*SC_REG6 Comment:*/
                `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
                1'b0, `SC_FIFO1__A0, `SC_FIFO0__A0,
                `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
                `SC_FB_NOCHN, `SC_CMP1_NOCHN,
                `SC_CMP0_NOCHN, /*SC_REG7 Comment:*/
                3'h00, `SC_FIFO_SYNC__ADD, 6'h00,    
                `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,   
                `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
                `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
            })) PrescalerDp(
                /*  input                   */  .clk(op_clock),
                /*  input   [02:00]         */  .cs_addr(cs_addr),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(z0),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(ppulse_equal),
                /*  output                  */  .cl1(ppulse_less),
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
            assign tmp_ppulse=ppulse_equal | ppulse_less;
        end
    endgenerate
    

    generate
        if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS_TDM)
        begin : b0
            cy_psoc3_dp #(.cy_dpconfig(dpconfig0))
            PRSdp_a(
             /*  input                   */  .clk(clk_TDM),
             /*  input   [02:00]         */  .cs_addr(prs_cs_addr),
             /*  input                   */  .route_si(si),
             /*  input                   */  .route_ci(ci),
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
             /*  output                  */  .cmsb(cmsb), 
             /*  output                  */  .so(so_a),  
             /*  output                  */  .f0_bus_stat(),
             /*  output                  */  .f0_blk_stat(),
             /*  output                  */  .f1_bus_stat(),
             /*  output                  */  .f1_blk_stat(),
             
            /* output [01:00] */ .ce0_reg(),
            /* output [01:00] */ .cl0_reg(),
            /* output [01:00] */ .z0_reg(),
            /* output [01:00] */ .ff0_reg(),
            /* output [01:00] */ .ce1_reg(),
            /* output [01:00] */ .cl1_reg(),
            /* output [01:00] */ .z1_reg(),
            /* output [01:00] */ .ff1_reg(),
            /* output [01:00] */ .ov_msb_reg(),
            /* output [01:00] */ .co_msb_reg(),
            /* output [01:00] */ .cmsb_reg(),
            /* output [01:00] */ .so_reg(so_reg1),
            /* output [01:00] */ .f0_bus_stat_reg(),
            /* output [01:00] */ .f0_blk_stat_reg(),
            /* output [01:00] */ .f1_bus_stat_reg(),
            /* output [01:00] */ .f1_blk_stat_reg(),
        
        
             /* input                    */  .ci(1'b0),
             /* output                   */  .co(),
             /* input                    */  .sir(1'b0),
             /* output                   */  .sor(),
             /* input                    */  .sil(1'b0),
             /* output                   */  .sol(),
             /* input                    */  .msbi(1'b0),
             /* output                   */  .msbo(),
             /* input [01:00]            */  .cei(2'b0),
             /* output [01:00]           */  .ceo(),
             /* input [01:00]            */  .cli(2'b0),
             /* output [01:00]           */  .clo(),
             /* input [01:00]            */  .zi(2'b0),
             /* output [01:00]           */  .zo(),
             /* input [01:00]            */  .fi(2'b0),
             /* output [01:00]           */  .fo(),
             /* input [01:00]            */  .capi(2'b0),
             /* output [01:00]           */  .capo(),
             /* input                    */  .cfbi(1'b0),
             /* output                   */  .cfbo(),
             /* input [07:00]            */  .pi(8'b0),
             /* output [07:00]           */  .po(sc_out_a[7:0])
            );
        end
        else if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_8BITS)
        begin : sC8
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
         `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
         `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
         `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
         `CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
         `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
         `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
         `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
         `CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
         `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
         `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
         `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
         `CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
         `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
         `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
         `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
         `CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
         `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
         `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
         `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
         `CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
         `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
         `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
         `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
         `CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
         `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
         `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
         `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
         `CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
         `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
         `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
         `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
         `CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
           8'hFF, 8'h00, /*SC_REG4 Comment: */
           8'hFF, 8'hFF, /*SC_REG5 Comment: */
         `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
         `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
         `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
         `SC_SI_A_ROUTE, /*SC_REG6 Comment: */
         `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
         1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
         `SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
         `SC_FB_NOCHN, `SC_CMP1_NOCHN,
         `SC_CMP0_NOCHN, /*SC_REG7 Comment: */
          10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
         `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
         `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
         })) PRSdp(
        /*  input                   */  .clk(op_clock),
        /*  input                   */  .reset(1'b0),
        /*  input   [02:00]         */  .cs_addr(prs_cs_addr),
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
        /*  output                  */  .cmsb(cmsb),
        /*  output                  */  .so(),
        /*  output                  */  .f0_bus_stat(),
        /*  output                  */  .f0_blk_stat(),
        /*  output                  */  .f1_bus_stat(),
        /*  output                  */  .f1_blk_stat()
        );
        end 
        else if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS) 
        begin : sC16
        cy_psoc3_dp16 #(.cy_dpconfig_a(
        {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
          8'hFF, 8'h00, /*SC_REG4 Comment: */
          8'hFF, 8'hFF, /*SC_REG5 Comment: */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_CHAIN,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_ROUTE, /*SC_REG6 Comment: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
         1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_ENBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*SC_REG7 Comment:MSB Chain */
         10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        }), .cy_dpconfig_b(
        {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG1 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
          8'hFF, 8'h00, /*SC_REG4 Comment: */
          8'hFF, 8'hFF, /*SC_REG5 Comment: */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_CHAIN,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_CHAIN, /*SC_REG6 Comment: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
         1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_ENBL, dpPOVal, `SC_MSB_NOCHN,
        `SC_FB_CHNED, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*SC_REG7 Comment:FB Chain and MSB enable */
         10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        })) PRSdp(
        /* input            */ .clk(op_clock),
        /* input  [02:00]   */ .cs_addr(prs_cs_addr),
        /* input            */ .route_si(1'b0),
        /* input            */ .route_ci(1'b0),
        /* input            */ .f0_load(1'b0),
        /* input            */ .f1_load(1'b0),
        /* input            */ .d0_load(1'b0),
        /* input            */ .d1_load(1'b0),
        /* output [01:00]   */ .ce0(),
        /* output [01:00]   */ .cl0(),
        /* output [01:00]   */ .z0(),
        /* output [01:00]   */ .ff0(),
        /* output [01:00]   */ .ce1(),
        /* output [01:00]   */ .cl1(),
        /* output [01:00]   */ .z1(),
        /* output [01:00]   */ .ff1(),
        /* output [01:00]   */ .ov_msb(),
        /* output [01:00]   */ .co_msb(),
        /* output [01:00]   */ .cmsb({cmsb, nc1}),
        /* output [01:00]   */ .so(),
        /* output [01:00]   */ .f0_bus_stat(),
        /* output [01:00]   */ .f0_blk_stat(),
        /* output [01:00]   */ .f1_bus_stat(),
        /* output [01:00]   */ .f1_blk_stat()
        );
        end
    endgenerate
   
    
    /**************************************************************************/
    /* Synchronous procedures                                                 */
    /**************************************************************************/ 

    /* PRS procedures */
    always @(posedge op_clock) tmp_ppulse_dly <= tmp_ppulse;
    always @(posedge op_clock) clock_detect_reg <= clock_detect;


    generate
         /* Time Multiplexing Logic */
        if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS_TDM)
        begin
            always @(posedge clk_TDM)
            begin
                if (resetx_all)
                begin
                    state <= 2'b11;
                end
                else 
                begin
                    case (state)
                        PRS_STATE_CALC_LOWER:   state <= PRS_STATE_SAVE_LOWER;
                        PRS_STATE_SAVE_LOWER:   state <= PRS_STATE_CALC_UPPER;
                        PRS_STATE_CALC_UPPER:   state <= PRS_STATE_SAVE_UPPER;
                        PRS_STATE_SAVE_UPPER:   state <= PRS_STATE_CALC_LOWER;
                        default:                state <= PRS_STATE_CALC_LOWER;
                    endcase
                end
            end
    
            /* sync so */
            always @(posedge clk_TDM)
            begin
                if (resetx_all)
                begin
                    so_reg = 1'b1;
                end
                else if (save_so)
                begin
                    so_reg = so;
                end
            end 
            
            /* ci */
            always @(posedge clk_TDM)
            begin
                if (resetx_all)
                begin
                    ci_temp = 1'b1;
                end
                else if (save)
                begin
                    ci_temp = ci;
                end    
            end 
            
            /* si */
            always @(posedge clk_TDM)
            begin
                if (resetx_all)
                begin
                    sc_temp = 1'b1;
                end
                else if (save)
                begin
                    sc_temp = sc_out[dpPOVal];
                end
            end 

            /* PRS Reset Forming */   
            always @(posedge clk_TDM or posedge reset1) if (reset1) resetx<=1'b1; else resetx<=1'b0;
        
            always @(posedge op_clock) resetx_reg <=resetx;
        end
    endgenerate
    


    /* Others procedures */

    always @(posedge op_clock)
    begin
        ppulse_dly <= ppulse;
    end


    always @(posedge op_clock)
    begin
    if (reset) cstate <= CAPSNS_CLK_GEN_IDLE;
    else
        case (cstate)
            CAPSNS_CLK_GEN_IDLE: if (syncen)      cstate <= CAPSNS_CLK_GEN_S1;
                                 else             cstate <= CAPSNS_CLK_GEN_IDLE;
            CAPSNS_CLK_GEN_S1:                    cstate <= CAPSNS_CLK_GEN_S2;
            CAPSNS_CLK_GEN_S2:   if (mesen)       cstate <= CAPSNS_CLK_GEN_S3;
                                 else             cstate <= CAPSNS_CLK_GEN_S2;
            CAPSNS_CLK_GEN_S3:   if (syncen==0)   cstate <= CAPSNS_CLK_GEN_IDLE;
                                 else             cstate <= CAPSNS_CLK_GEN_S3;
            default:                              cstate <= CAPSNS_CLK_GEN_IDLE;
        endcase
    end    
    
  
    /**************************************************************************/
    /* Combinatinal procedures                                                */
    /**************************************************************************/
    
    /* PRS procedures */
    
    /* Chain Datapathes*/
    generate
        if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS_TDM)
        begin
            assign so = so_a;  
            assign sc_out = sc_out_a;
        end
    endgenerate
  
 
    generate
        if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS_TDM)
        begin
            assign prs_cs_addr = {resetx_all, state[1:0]};
            assign bitstream = ci_temp;
            assign ci = dcfg ? sc_out[dpPOVal] : ci_temp;
            assign si = dcfg ? so_reg : sc_temp;
            /* PRS Reset Forming */   
            assign resetx_all = resetx | resetx_reg;
        end
        else
        begin
            assign prs_cs_addr = {reset1,1'b0, clock_detect_reg};
            assign bitstream = cmsb;
        end
    endgenerate

    generate
        if (PrescalerOptions == CAPSNS_CLK_GEN_PRESCALER_NONE)
        begin
            assign clock_detect = 1'b1;
            assign prescaler = clock;
        end
        else
        begin
            assign clock_detect = tmp_ppulse & ~tmp_ppulse_dly;
            assign prescaler = tmp_ppulse;
        end
    endgenerate
        
    
    assign save = ((state == PRS_STATE_SAVE_LOWER) | (state == PRS_STATE_SAVE_UPPER));
    assign save_so = ~save;
    assign dcfg = ((state == PRS_STATE_CALC_UPPER) | (state == PRS_STATE_SAVE_UPPER));


    /* Others procedures */
    
    assign tmp_pclk = (PrsOptions==CAPSNS_CLK_GEN_PRS_OPTIONS_NONE) ? prescaler: bitstream;
    assign shield = (IdacOptions==CAPSNS_CLK_GEN_IDAC_OPTIONS_SOURCE) ? tmp_pclk : (~tmp_pclk);
    
    assign dpulse = tmp_dpulse;  /* digatal clock */
    assign ppulse = tmp_pclk;    /* precharge clock*/
    assign cs_addr = {1'b0, z0, reset1};
            
    assign reset1      = cstate[0];
    assign reset2      = cstate[1];
    assign work_en     = cstate[2];
    
    assign mesrst = reset2;
    assign tmp_start0 = work_en & ch0en;
    assign tmp_start1 = work_en & ch1en;
    
    assign start0 = tmp_start0;
    assign start1 = tmp_start1;
    
    assign syncen   = control[0];
    assign mesen    = control[1];
    assign ch0en    = control[2];
    assign ch1en    = control[3];

    /* debug port*/
    generate
        if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_NONE)
            assign d[23:16] = {5'b0, cstate[2:0]};
        else
            begin
                assign d[23:16] = {5'b0, cstate[2:0]};    /*  B[7:0]  */
                if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS_TDM)
                begin
                    assign d[15:8] = {save, save_so,dcfg, resetx_all,1'b0, so_reg, ci_temp, sc_temp}; /*  A[15:8] */
                    assign d[7:0]  = {tmp_ppulse,state,reset1,ci_temp,clock_detect_reg,syncen,clock}; /*  A[7:0]  */
                end
                else
                begin
                    assign d[15:8] = {8'b0}; /*  A[15:8] */
                    assign d[7:0]  = {tmp_ppulse,2'b0,reset1,cmsb,clock_detect_reg,syncen,clock}; /*  A[7:0]  */
                end
            end
    endgenerate
 
    
    
endmodule

`endif /* CapSense_CSD_ClockGen_v2_0_V ALREADY_INCLUDED */
