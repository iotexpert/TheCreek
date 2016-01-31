/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef CapSense_CSD_MeasureCh_v2_0_V_ALREADY_INCLUDED
`else
`define CapSense_CSD_MeasureCh_v2_0_V_ALREADY_INCLUDED

module CapSense_CSD_MeasureCh_v2_0
(
    input    wire        clock,
    input    wire        reset,
    input    wire        start,
    input    wire        pulse,
    input    wire        cmp_in,
    input    wire        enable,
    output   wire        ioff,
    output   wire        interrupt,
    output   wire [7:0]  d
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
    localparam CAPSNS_MEASURE_CH_END    = 4'd6;  /* End*/
    
    
    /* Internal signals */
    wire zw0,zw1;    /* Window zerro detect signal */
    wire zc0,zc1;    /* Counter zerro detect signal */
    wire int;
    wire op_clock;  /* Clock to operate the component */
    wire [2:0] cs_addr_win;
    wire [2:0] cs_addr_cnt;
    wire cnt_enable;

    
    wire load_enable;
    wire win_enable;
    reg cmp_in_reg;
    reg [3:0] wndState;
    reg int_reg;
    /**************************************************************************/
    /* Hierarchy - instantiating another module                               */
    /**************************************************************************/
 

    generate
        if(ImplementationType == CAPSNS_MEASURE_IMPLEMENTATION_FF)
        begin: FF
            wire Wndw_enable;
            wire Cntr_enable;

            assign Wndw_enable=(win_enable||load_enable);
            assign Cntr_enable=(cnt_enable||load_enable);

            cy_psoc3_timer_v1_0 Window
            (
                /* input */     .clock(clock),  /* Use clock because there is not clock_enable componnent for FF*/
                /* input */     .kill(1'b0),
                /* input */     .enable(Wndw_enable),
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
                /* input */     .enable(Cntr_enable),
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
            .cy_dpconfig_a(
            {
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG0 Comment:
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG1 Comment:
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG2 Comment:
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG3 Comment:
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG4 Comment:
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG5 Comment:
                `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_A0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC__ALU,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG6 Comment:
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG7 Comment:
                 8'h00, 8'h00,  //SC_REG4 Comment:
                 8'h00, 8'h00,  //SC_REG5 Comment:
                `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
                `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
                `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
                `SC_SI_A_DEFSI, //SC_REG6 Comment:
                `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
                 1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
                `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
                `SC_FB_NOCHN, `SC_CMP1_NOCHN,
                `SC_CMP0_NOCHN, //SC_REG7 Comment:
                 10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
                `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
                `SC_WRK16CAT_DSBL //SC_REG8 Comment:
            }
            )) Window(
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
            .cy_dpconfig_a(
            {
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG0 Comment:
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG1 Comment:
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG2 Comment:
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG3 Comment:
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG4 Comment:
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG5 Comment:
                `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_A0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC__ALU,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG6 Comment:
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A1,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, //CS_REG7 Comment:
                 8'h00, 8'h00,  //SC_REG4 Comment:
                 8'h00, 8'h00,  //SC_REG5 Comment:
                `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
                `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
                `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
                `SC_SI_A_DEFSI, //SC_REG6 Comment:
                `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
                 1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
                `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
                `SC_FB_NOCHN, `SC_CMP1_NOCHN,
                `SC_CMP0_NOCHN, //SC_REG7 Comment:
                 10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
                `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
                `SC_WRK16CAT_DSBL //SC_REG8 Comment:
            }
            )) Counter(
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

            assign cs_addr_win = (load_enable==1)
            ? (3'b001)
                : ((win_enable==1) 
                    ? ({1'b1,zw0,zw1}) : (3'b000));
            
            assign cs_addr_cnt = (load_enable==1) 
            ? (3'b001)
                : ((cnt_enable==1) 
                    ? ({1'b1,zc0,zc1}) : (3'b000));
            
            assign int = (zw0 && zw1);
        end
    endgenerate
    
    /* regiser cmp_in signal */
    generate
        if(IdacOptions == CAPSNS_IDAC_OPTIONS_SRC)
        begin
            always @(posedge op_clock)
            if (pulse) 
            begin
                cmp_in_reg <= cmp_in;  
            end
        end
        else
        begin
            always @(posedge op_clock)
            if (pulse)
            begin
                cmp_in_reg <= ~cmp_in; 
            end
        end
    endgenerate


    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */ .clock_in(clock),
        /* input  */ .enable(enable),
        /* output */ .clock_out(op_clock)
    );

    
    /**************************************************************************/
    /* Synchronous procedures                                                 */
    /**************************************************************************/

    always @(posedge op_clock) int_reg<=int;

    always @(posedge op_clock)
    begin
        if (reset)
        begin
            wndState <= CAPSNS_MEASURE_CH_IDLE;
        end
        else
        begin
            
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
    end
    
    /**************************************************************************/
    /* Combinatinal procedures                                                */
    /**************************************************************************/

    assign load_enable = wndState[0];
    assign win_enable = wndState[2] & ~int_reg;

    assign interrupt =  int & ~int_reg;
    assign ioff = cmp_in_reg;
    assign cnt_enable = ~cmp_in_reg & win_enable;

    /* Debug port */
    assign d = {interrupt,start,cmp_in,win_enable,load_enable,pulse,reset,cnt_enable};    /*  B[7:0]  */

endmodule

`endif /* CapSense_CSD_MeasureCh_v2_0_V ALREADY_INCLUDED */

