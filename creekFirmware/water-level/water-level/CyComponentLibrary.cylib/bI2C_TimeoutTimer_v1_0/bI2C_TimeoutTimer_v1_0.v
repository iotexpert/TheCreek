/*******************************************************************************
* File Name: bI2C_TimeoutTimer_v1_0.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  This file provides a base model of the I2C SCL and SDA Timeout feature required
*  for SM/PM bus compliance.
*
*  Status Register Definition
*  +=======+---------+-----------+-----------+-----------+-----------+-----------+-----------+-----------+
*  |  Bit  |    7    |     6     |     5     |     4     |     3     |     2     |      1    |    0      |
*  +=======+---------+-----------+-----------+-----------+-----------+-----------+-----------+-----------+
*  | Mode  |   none  |   none    |   none    |transparent|transparent|transparent|transparent|transparent|
*  +=======+---------+-----------+-----------+-----------+-----------+-----------+-----------+-----------+
*  | Name  |   n/a   |   n/a     |   n/a     | interrupt |  sda_intr |  scl_intr |  sda_reg  |  scl_reg  |
*  +=======+---------+-----------+-----------+-----------+-----------+-----------+-----------+-----------+
*
*   interrupt =>  0 = interrupt doesn't trigger
*                 1 = interrupt triggers
*
*   sda_intr  =>  0 = interrupt source is not sda timeout
*                 1 = interrupt source is sda timeout
*
*   scl_intr  =>  0 = interrupt source is not scl timeout
*                 1 = interrupt source is scl timeout
*
*   sda_reg   =>  0 = registered input of sda line is low
*                 1 = registered input of sda line is high
*
*   scl_reg   =>  0 = registered input of scl line is low
*                 1 = registered input of scl line is high
*
*  Control Register Definition
*  +=======+---------+-----------+-----------+-----------+-----------+-----------+-----------+-----------+
*  |  Bit  |    7    |     6     |     5     |     4     |     3     |     2     |      1    |    0      |
*  +=======+---------+-----------+-----------+-----------+-----------+-----------+-----------+-----------+
*  | Name  |   n/a   |   n/a     |   n/a     |    n/a    |    n/a    |    n/a    |    n/a    |   enable  |
*  +=======+---------+-----------+-----------+-----------+-----------+-----------+-----------+-----------+
*
*   reset   =>  0 = timeout enabled and start counting.
*               1 = tiemout is in reset state.
*
********************************************************************************
*           Data Path register definitions
********************************************************************************
*  INSTANCE NAME: dpSclTimer or dpSdaTimer.
*  DESCRIPTION: Use for count timeout for scl of sda lines
*  REGISTER USAGE:
*   F0 => Low value of timer period.
*   F1 => High value of timer period.
*   D0 => Count base to load in low period when high is decremented.
*   D1 => na
*   A0 => Counter of low period.
*   A1 => Counter of high period.
*
********************************************************************************
*           I*O Signals:
********************************************************************************
*    Name              Direction       Description
*    clock               input          clock the component is running on
*    reset               input          reset signal
*    scl_in              input          I2C input clock signal
*    sda_in              input          I2C input data signal
*    interrupt           output         interrupt signal
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef bI2C_TimeoutTimer_v1_0_V_ALREADY_INCLUDED
`else
`define bI2C_TimeoutTimer_v1_0_V_ALREADY_INCLUDED

module bI2C_TimeoutTimer_v1_0
(
    input  wire clock,
    input  wire scl_in,
    input  wire sda_in,
    output wire interrupt
);

    /***************************************************************************
    *              UDB Version Parameters
    ***************************************************************************/
    localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);
    localparam CY_UDB_V1 = (!CY_UDB_V0);


    /***************************************************************************
    *              Parameters
    ***************************************************************************/
    parameter SclTimeoutEnabled  = 1;
    parameter SdaTimeoutEnabled  = 1;
    parameter PrescalerEnabled   = 0;


    /***************************************************************************
    *            Local Parameter
    ***************************************************************************/
    localparam DEFAULT_PERIOD_LO    = 8'hFF;
    localparam DEFAULT_PERIOD_HI    = 8'hFF;
    localparam DEFAULT_ADDER        = 8'hFF;

    localparam TIMEOUT_CTRL_ENABLE  = 1'h00;

    localparam DEFAULT_PRESCALER    = 7'h03;


    /***************************************************************************
    * Instantiation of udb_clock_enable
    ****************************************************************************
    * The udb_clock_enable primitive component allows to support clock enable
    * mechanism and specify the intended synchronization behaviour for the clock
    * result (operational clock).
    ****************************************************************************/
    wire op_clk;    /* operational clock */

    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    ClkSync(
        /* input  */ .clock_in  (clock),
        /* input  */ .enable    (1'b1),
        /* output */ .clock_out (op_clk)
    );


    /***************************************************************************
    * Instantiation of Control Register
    ***************************************************************************/
    wire [7:0] ctrl;

    generate
    if (CY_UDB_V0)
    begin: AsyncCtl
        cy_psoc3_control #(.cy_force_order(`TRUE))
        CtrlReg (
            /* output [07:00] */ .control (ctrl)
        );
    end /* AsyncCtl */
    else
    begin: SyncCtl
        cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFF))
        CtrlReg (
            /* input          */ .clock   (op_clk),
            /* output [07:00] */ .control (ctrl)
        );
    end /* SyncCtl */
    endgenerate


    /* Reset and enable control signals */
    reg ctrl_reset;
    always @(posedge op_clk)
    begin
        ctrl_reset <= ~ctrl[TIMEOUT_CTRL_ENABLE];
    end

    wire scl_reset;
    wire sda_reset;
    wire scl_reset_final;
    wire sda_reset_final;

    assign scl_reset_final = (ctrl_reset | scl_reset);
    assign sda_reset_final = (ctrl_reset | sda_reset);


    /***************************************************************************
    * Instantiate the status register and interrupt hook
    ***************************************************************************/
    reg scl_in_reg;
    reg sda_in_reg;

    reg scl_intr_level;
    reg sda_intr_level;

    wire [6:0] status;

    localparam SDA_IN_REG  = 3'd4;
    localparam SCL_IN_REG  = 3'd3;
    localparam EMPTY       = 3'd2;
    localparam SDA_TIMEOUT = 3'd1;
    localparam SCL_TIMEOUT = 3'd0;

    assign status[6:5]         = 2'b0;
    assign status[SCL_IN_REG]  = (SclTimeoutEnabled) ? scl_in_reg : 1'b0;
    assign status[SDA_IN_REG]  = (SdaTimeoutEnabled) ? sda_in_reg : 1'b0;
    assign status[EMPTY]       = 1'b0;
    assign status[SCL_TIMEOUT] = (SclTimeoutEnabled) ? scl_intr_level : 1'b0;
    assign status[SDA_TIMEOUT] = (SdaTimeoutEnabled) ? sda_intr_level : 1'b0;

    cy_psoc3_statusi #(.cy_force_order (`TRUE), .cy_md_select (7'h00), .cy_int_mask (7'h00))
    StsReg(
        /* input          */ .clock    (op_clk),
        /* input          */ .reset    (1'b0),
        /* input  [06:00] */ .status   (status),
        /* output         */ .interrupt(interrupt)
    );


    /***************************************************************************
    *       7-bit Down Counter implementation
    ***************************************************************************/
    wire scl_tc;
    wire sda_tc;
    wire scl_enable;
    wire sda_enable;


    localparam SclDividerEnabled = ((PrescalerEnabled) && (SclTimeoutEnabled));
    localparam SdaDividerEnabled = ((PrescalerEnabled) && (SdaTimeoutEnabled));

    assign scl_enable = (SclDividerEnabled) ? (scl_tc) : (1'b1);
    assign sda_enable = (SdaDividerEnabled) ? (sda_tc) : (1'b1);

    generate
    if(SclDividerEnabled)
    begin : SclPrescaler
        cy_psoc3_count7 #(.cy_period(DEFAULT_PRESCALER), .cy_route_ld(`TRUE))
        prScl (
            /*  input             */  .clock (op_clk),
            /*  input             */  .reset (1'b0),
            /*  input             */  .load  (scl_reset_final),
            /*  input             */  .enable(1'b1),
            /*  output  [06:00]   */  .count (),
            /*  output            */  .tc    (scl_tc)
        );
    end
    endgenerate  /* SclDividerEnabled */

    generate
    if(SdaDividerEnabled)
    begin : SdaPrescaler
        cy_psoc3_count7 #(.cy_period(DEFAULT_PRESCALER), .cy_route_ld(`TRUE))
        prSda (
            /*  input             */  .clock (op_clk),
            /*  input             */  .reset (1'b0),
            /*  input             */  .load  (sda_reset_final),
            /*  input             */  .enable(1'b1),
            /*  output  [06:00]   */  .count (),
            /*  output            */  .tc    (sda_tc)
        );
    end
    endgenerate  /* SdaDividerEnabled */


    /***************************************************************************
    * TimerScl
    ****************************************************************************
    * Datapath is 16 bits time multiplexed counter.
    * User writes pariod F0 (LOW) and F1 (HIGH) and base to D0.
    ****************************************************************************/
    generate
    if(SclTimeoutEnabled)
    begin : Scl

        /***************************************************************************
        *       Input async singnals synchronization
        ***************************************************************************/
        always @(posedge op_clk)
        begin
            scl_in_reg <= scl_in;
        end


        /***************************************************************************
        *       Reset logic implementation
        ***************************************************************************/
        assign scl_reset = scl_in_reg;


        /***************************************************************************
        *       Common scl FSM implementation
        ***************************************************************************/
        localparam SCL_RELOAD = 2'h0;    /* RELOAD */
        localparam SCL_COUNT  = 2'h1;    /* COUNT  */

        wire scl_z0_detect;
        wire scl_z1_detect;

        reg scl_state;

        always @(posedge op_clk)
        begin
            if(scl_reset_final)
            begin
                scl_state <= SCL_RELOAD;

                if(ctrl_reset)
                begin
                    scl_intr_level <= 1'b0;
                end
            end
            else
            begin
                case(scl_state)

                    SCL_RELOAD:
                        if(scl_intr_level)
                            scl_state <= SCL_RELOAD;
                        else
                            scl_state <= SCL_COUNT;

                    SCL_COUNT:
                        if((scl_enable) && (scl_z1_detect & scl_z0_detect))
                        begin
                            scl_state       <= SCL_RELOAD;
                            scl_intr_level  <= 1'b1;
                        end
                        else
                        begin
                            scl_state <= SCL_COUNT;
                        end
                endcase
            end
        end

        wire [2:0] scl_dp_addr = {scl_state, scl_z0_detect, scl_enable};

        cy_psoc3_dp8 #(.a0_init_a(DEFAULT_PERIOD_LO), .a1_init_a(DEFAULT_PERIOD_HI), .d0_init_a(DEFAULT_ADDER),
        .cy_dpconfig_a(
        {
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0:       Load period to A0 and A1 from F0 and F1*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1:       Load period to A0 and A1 from F0 and F1*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2:       Load period to A0 and A1 from F0 and F1*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3:       Load period to A0 and A1 from F0 and F1*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4:       Idle*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5:       Decrement A0 LSB of counter*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6:       Idle*/
            `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7:       Decrement A1 MSB of counter*/
              8'hFF, 8'h00,    /*CFG9:                     */
              8'hFF, 8'h7F,    /*CFG11-10:                    Address Mask */
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_ENBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_ROUTE, /*CFG13-12:                     */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14:                     */
            3'h00, `SC_FIFO_SYNC_NONE, 6'h00,
            `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:                     */
        })) dpScl (
            /* input          */ .reset         (1'b0),
            /* input          */ .clk           (op_clk),
            /* input  [02:00] */ .cs_addr       (scl_dp_addr),
            /* input          */ .route_si      (1'b0),
            /* input          */ .route_ci      (1'b0),
            /* input          */ .f0_load       (1'b0),
            /* input          */ .f1_load       (1'b0),
            /* input          */ .d0_load       (1'b0),
            /* input          */ .d1_load       (1'b0),
            /* output         */ .ce0           (),
            /* output         */ .ce0_reg       (),
            /* output         */ .cl0           (),
            /* output         */ .z0            (scl_z0_detect),
            /* output         */ .z0_reg        (),
            /* output         */ .ff0           (),
            /* output         */ .ce1           (),
            /* output         */ .cl1           (),
            /* output         */ .z1            (scl_z1_detect),
            /* output         */ .z1_reg        (),
            /* output         */ .ff1           (),
            /* output         */ .ov_msb        (),
            /* output         */ .co_msb        (),
            /* output         */ .cmsb          (),
            /* output         */ .so            (),
            /* output         */ .f0_bus_stat   (),
            /* output         */ .f0_blk_stat   (),
            /* output         */ .f1_bus_stat   (),
            /* output         */ .f1_blk_stat   ()
        );
    end
    endgenerate  /* Scl */


    /***************************************************************************
    * TimerSda
    ****************************************************************************
    * Datapath is 16 bits time multiplexed counter.
    * User writes pariod F0 (LOW) and F1 (HIGH) and base to D0.
    ****************************************************************************/
    generate
    if(SdaTimeoutEnabled)
    begin: Sda

        /***************************************************************************
        *       Input async singnals synchronization
        ***************************************************************************/
        always @(posedge op_clk)
        begin
            sda_in_reg <= sda_in;
        end


        /***************************************************************************
        *       Reset logic implementation
        ***************************************************************************/
        assign sda_reset = sda_in_reg;


        /***************************************************************************
        *       Sda FSM implementation
        ***************************************************************************/
        localparam SDA_RELOAD = 2'h0;    /* RELOAD */
        localparam SDA_COUNT  = 2'h1;    /* COUNT  */

        wire sda_z0_detect;
        wire sda_z1_detect;

        reg sda_state;

        always @(posedge op_clk)
        begin
            if(sda_reset_final)
            begin
                sda_state <= SDA_RELOAD;

                if(ctrl_reset)
                begin
                    sda_intr_level  <= 1'b0;
                end
            end
            else
            begin
                case(sda_state)

                    SDA_RELOAD:
                      if(sda_intr_level)
                        sda_state <= SDA_RELOAD;
                      else
                        sda_state <= SDA_COUNT;

                    SDA_COUNT:
                        if((sda_enable) && (sda_z1_detect & sda_z0_detect))
                        begin
                            sda_state       <= SDA_RELOAD;
                            sda_intr_level  <= 1'b1;
                        end
                        else
                        begin
                            sda_state <= SDA_COUNT;
                        end
                endcase
            end
        end

        wire [2:0] sda_dp_addr = {sda_state, sda_z0_detect, sda_enable};

        cy_psoc3_dp8 #(.a0_init_a(DEFAULT_PERIOD_LO), .a1_init_a(DEFAULT_PERIOD_HI), .d0_init_a(DEFAULT_ADDER),
        .cy_dpconfig_a(
        {
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0:       Load period to A0 and A1 from F0 and F1*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1:       Load period to A0 and A1 from F0 and F1*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2:       Load period to A0 and A1 from F0 and F1*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3:       Load period to A0 and A1 from F0 and F1*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4:       Idle*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5:       Decrement A0 LSB of counter*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6:       Idle*/
            `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7:       Decrement A1 MSB of counter*/
              8'hFF, 8'h00,    /*CFG9:                     */
              8'hFF, 8'h7F,    /*CFG11-10:                    Address Mask */
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_ENBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_ROUTE, /*CFG13-12:                     */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14:                     */
            3'h00, `SC_FIFO_SYNC_NONE, 6'h00,
            `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:                     */
        })) dpSda(
            /* input          */ .reset         (1'b0),
            /* input          */ .clk           (op_clk),
            /* input  [02:00] */ .cs_addr       (sda_dp_addr),
            /* input          */ .route_si      (1'b0),
            /* input          */ .route_ci      (1'b0),
            /* input          */ .f0_load       (1'b0),
            /* input          */ .f1_load       (1'b0),
            /* input          */ .d0_load       (1'b0),
            /* input          */ .d1_load       (1'b0),
            /* output         */ .ce0           (),
            /* output         */ .ce0_reg       (),
            /* output         */ .cl0           (),
            /* output         */ .z0            (sda_z0_detect),
            /* output         */ .z0_reg        (),
            /* output         */ .ff0           (),
            /* output         */ .ce1           (),
            /* output         */ .cl1           (),
            /* output         */ .z1            (sda_z1_detect),
            /* output         */ .z1_reg        (),
            /* output         */ .ff1           (),
            /* output         */ .ov_msb        (),
            /* output         */ .co_msb        (),
            /* output         */ .cmsb          (),
            /* output         */ .so            (),
            /* output         */ .f0_bus_stat   (),
            /* output         */ .f0_blk_stat   (),
            /* output         */ .f1_bus_stat   (),
            /* output         */ .f1_blk_stat   ()
        );
    end
    endgenerate  /* Sda */

endmodule /* bI2C_TimeoutTimer_v1_0 */
`endif /* bI2C_TimeoutTimer_v1_0_V_ALREADY_INCLUDED */



