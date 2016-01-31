/*******************************************************************************
* File Name: bLIN_v1_10.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* The Base LIN Slave module implementation is done with UDB's. It provides the
* following functionality:
* Break Threshold detection, Synch Field measurement/detection, Edge detection,
* Bus Inactivity Time measurement.
*
*------------------------------------------------------------------------------
*                 Control and Status Register definitions
*------------------------------------------------------------------------------
*
*  Control Register Definition
*  +=====+-------+-------+-------+--------+--------+-------+--------+----------+
*  | Bit |   7   |   6   |   5   |   4    |   3    |   2   |   1    |     0    |
*  +=====+-------+-------+-------+--------+--------+-------+--------+----------+
*  |Desc |                                         |ctrl_  |ctrl_   |ctrl_     |
   |     |                unused                   |rxd_dis|txd_dis |blin_start|
*  +=====+-------+-------+-------+--------+--------+-------+--------+----------+
*
*    ctrl_blin_start     =>    0 = bLIN_v1_10 component is disabled
*                              1 = bLIN_v1_10 component is active
*
*    ctrl_txd_dis        =>    0 = UART txd output is connected to txd_out
*                              1 = txd_out is connected to 1'b0
*
*    ctrl_rxd_dis        =>    0 = R-input of the rx_mux_ff is inactive
*                              1 = R-input of the rx_mux_ff is active (UART RXD input
*                                  is connected to 1'b1)
*
*   Status Interrupt Register Definition
*  +=======+---------+---+---+---+------------+------------+-----------+------------+
*  |  Bit  |    7    | 6 | 5 | 4 |     3      |     2      |     1     |      0     |
*  +=======+---------+---+---+---+------------+------------+-----------+------------+
*  | Desc  |interrupt|   unused  |synch detect|inact detect|edge detect|break detect|
*  +=======+---------+------+----+------------+------------+-----------+------------+
*
*    break detect     =>  0 = Break Threshold is not detected
*                         1 = Break Threshold is detected
*
*    edge detect      =>  0 = RXD edge is not detected
*                         1 = RXD edge is detected
*
*    inact detect     =>  0 = Bus Inactivity Threshold is not detected
*                         1 = Bus Inactivity Threshold is detected
*
*    synch detect     =>  0 = Synch Field is not detected/measured.
*                         1 = Synch Field is detected/measured.
*
********************************************************************************
* Data Path register definitions
********************************************************************************
* bLIN_v1_10: LINDp
* DESCRIPTION: LINDp is used to implement Break/Synch Filed Detector
* REGISTER USAGE:
* F0 => is used to accumulate low bits' values of the Synch Filed
* F1 => not used
* D0 => contains Break Threshold value
* D1 => not used
* A0 => is used to detect Break Threshold and to accumulate low bits' values of the Synch Filed
* A1 => is used to accumulate high bits' values of the Synch Field
*
********************************************************************************
* bLIN_v1_10: BusInactDp
* DESCRIPTION:
* REGISTER USAGE: BusInactDp is used to implement Bus Inactivity Timer
* F0 => not used
* F1 => not used
* D0 => contains the value to be compared with A0
* D1 => contains the value to be compared with A1
* A0 => is used to accumulate Bus Inactivity Time after CMP_A1_D1 has been detected
* A1 => is used to accumulate Bus Inactivity Time
*
********************************************************************************
* I*O Signals:
********************************************************************************
*    name              direction       Description
*    interrupt         output          interrupt output
*    rxd_out           output          rxd output data line
*    txd_out           output          txd output line
*    clock_out         output          clock output for UART
*    clock             input           component 16*Baud Rate clock
*    rxd               input           rxd input from the bus
*    txd               input           txd input from the UART
*
********************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef bLIN_v1_10_V_ALREADY_INCLUDED
`else
`define bLIN_v1_10_V_ALREADY_INCLUDED

module bLIN_v1_10(
    output interrupt,
    output reg rxd_out,
    output reg txd_out,
    output clock_out,
    input clock,
    input rxd,
    input txd
);

    parameter BusInactivityEnabled = 0;
    parameter AutoBaudRateSync     = 0;

    wire clk_fin;
    reg break_flag;
    wire cmp_a0_d0;
    wire break_pulse;
    wire ff_res;
    wire f0_fifo_full;
    wire f0_load;
    wire f1_load;
    wire a0_is_zero;
    wire [7:0] control;
    wire [6:0] status;
    wire ctrl_blin_start;
    wire ctrl_txd_dis;
    wire ctrl_rxd_dis;
    wire direct_edge;
    wire edge_detect;
    wire div_clk;
    wire inv_dff_out;
    wire set_rxd_en;
    wire ctrl_clk_fin;
    wire inact_detect_sts;
    wire f1_not_empty;
    reg rxd_reg;
    reg txd_out_reg;
    reg rxd_out_reg;
    reg rxd_mux_ctrl;
    /* Break/Synch State Machine state names */
    localparam LIN_STATE_IDLE              = 3'h0;
    localparam LIN_STATE_BREAK_DETECT      = 3'h1;
    localparam LIN_STATE_DELIMITER_DETECT  = 3'h3;
    localparam LIN_STATE_SYNC_START_DETECT = 3'h2;
    localparam LIN_STATE_SYNC_HIGH_MEASURE = 3'h6;
    localparam LIN_STATE_SYNC_LOW_MEASURE  = 3'h7;
    localparam LIN_STATE_CLEAR_A0          = 3'h5;

    /* Bus Inactivity Timer's State Machine state names */
    localparam LIN_INACT_STATE_IDLE             = 3'h0;
    localparam LIN_INACT_STATE_INC_A0_FF_DETECT = 3'h1;
    localparam LIN_INACT_STATE_INC_A1_CMP_D1    = 3'h3;
    localparam LIN_INACT_STATE_INC_A0_CMP_D0    = 3'h2;
    localparam LIN_INACT_STATE_CLEAR_A0_A1      = 3'h6;

    reg [2:0] state;

    /* Datapaths' configurations */
    localparam lin_dp8_cfg = {
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0: STATE_IDLE*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1: STATE_BREAK_DETECT*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2: STATE_SYNC_START_DETECT*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3: STATE_DELIMITER_DETECT*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4: UNUSED*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5: STATE_CLEAR_A0*/
        `CS_ALU_OP__INC, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6: STATE_SYNC_HIGH_MEASURE*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7: STATE_SYNC_LOW_MEASURE*/
        8'hFF, 8'hFF,    /*CFG9:       */
        8'hFF, 8'hFF,    /*CFG11-10:       */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12:       */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
        1'h0, `SC_FIFO1__A1, `SC_FIFO0__A0,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:       */
        3'h00, `SC_FIFO_SYNC_NONE, 6'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO__EDGE,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL  /*CFG17-16:       */
        };

    localparam bus_inact_dp8_cfg = {
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0: STATE_IDLE*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1: STATE_INC_A0_FF_DETECT*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2: STATE_INC_A0_CMP_D0*/
        `CS_ALU_OP__INC, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3: STATE_INC_A1_CMP_D1*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4: UNUSED*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5: UNUSED*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6: STATE_CLEAR A0_A1*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7: UNUSED*/
        8'hFF, 8'hFF,    /*CFG9:       */
        8'hFF, 8'hFF,    /*CFG11-10:       */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12:     */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
        1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:     */
        3'h00, `SC_FIFO_SYNC_NONE, 6'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL  /*CFG17-16:     */
        };

    /*************************************************************************
    * UDB revisions
    **************************************************************************/
    localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);
    localparam CY_UDB_V1 = (!CY_UDB_V0);

    /* Clock Enable primitive instantiation */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    ClkEn (
        .clock_in(clock),
        .enable(1'b1),
        .clock_out(clk_fin)
    );

    /* clock divider */
    cy_dff clk_div(
    .d(inv_dff_out),
    .clk(clock),
    .q(div_clk)
    );

    assign inv_dff_out = ~div_clk;

    /* Break detection process*/
    always@(posedge clk_fin)
    begin
        if(ctrl_blin_start)
        begin
            if(break_pulse)
            begin
                break_flag <= 1'b1;
            end
            else if(ff_res)
            begin
                break_flag <= 1'b0;
            end
            else begin
                break_flag <= break_flag;
            end
        end
        else begin
            break_flag <= 1'b0;
        end
    end

    /* clock for the UART component */
    assign clock_out = div_clk;

    /* S-input signal for rx_mux_ff */
    generate
    if(!AutoBaudRateSync)
    begin
        assign set_rxd_en = ((state == LIN_STATE_DELIMITER_DETECT)& ctrl_blin_start);
    end
    else begin
        assign set_rxd_en = (f0_fifo_full & ctrl_blin_start);
    end
    endgenerate

    /* Rxd MUX control signal */
    always@(posedge clk_fin)
    begin
        if(ctrl_blin_start)
        begin
            if(set_rxd_en)
            begin
                rxd_mux_ctrl <= 1'b1;
            end
            else if(ctrl_rxd_dis)
            begin
                rxd_mux_ctrl <= 1'b0;
            end
            else begin
                rxd_mux_ctrl <= rxd_mux_ctrl;
            end
        end
        else begin
            rxd_mux_ctrl <= 1'b0;
        end
    end

    always@(posedge clk_fin)
    begin
        txd_out <= (ctrl_txd_dis) ? 1'b0 : txd;
        rxd_out <= (rxd_mux_ctrl) ? rxd  : 1'b1;
    end

    assign ff_res  = (state == LIN_STATE_DELIMITER_DETECT);
    assign f0_load = ((state == LIN_STATE_SYNC_HIGH_MEASURE) && (~a0_is_zero));
    assign f1_load = (f0_fifo_full && (state == LIN_STATE_SYNC_HIGH_MEASURE));
    assign break_pulse  = (((state == LIN_STATE_BREAK_DETECT) ||
                            (state == LIN_STATE_SYNC_START_DETECT)) && (cmp_a0_d0 == 1'b1));

    /* LINDp(Break/Synch) State Machine  */
    generate
    if(AutoBaudRateSync)
    begin: Abrsync
        always@(posedge clk_fin)
        begin
            if(ctrl_blin_start)
            begin
                case(state)
                    LIN_STATE_IDLE:
                    begin
                        if(rxd)
                        begin
                            state <= LIN_STATE_IDLE;
                        end
                        else
                        begin
                            state <= LIN_STATE_BREAK_DETECT;
                        end
                    end

                    LIN_STATE_BREAK_DETECT:
                    begin
                        if(!rxd)
                        begin
                            state <= LIN_STATE_BREAK_DETECT;
                        end
                        else begin
                            if(break_flag)
                            begin
                                state <= LIN_STATE_DELIMITER_DETECT;
                            end
                            else begin
                                state <= LIN_STATE_IDLE;
                            end
                        end
                    end

                    LIN_STATE_DELIMITER_DETECT:
                    begin
                        if(rxd)
                        begin
                            state <= LIN_STATE_DELIMITER_DETECT;
                        end
                        else begin
                               state <= LIN_STATE_SYNC_START_DETECT;
                        end
                    end

                    LIN_STATE_SYNC_START_DETECT:
                    begin
                        if(rxd)
                        begin
                            state <= LIN_STATE_CLEAR_A0;
                        end
                        else begin
                            if(break_flag)
                            begin
                                state <= LIN_STATE_DELIMITER_DETECT;
                            end
                            else begin
                                state <= LIN_STATE_SYNC_START_DETECT;
                            end

                        end
                    end

                    LIN_STATE_SYNC_HIGH_MEASURE:
                    begin
                        if(~f0_fifo_full)
                        begin
                            if(rxd)
                            begin
                                state <= LIN_STATE_SYNC_HIGH_MEASURE;
                            end
                            else begin
                                state <= LIN_STATE_SYNC_LOW_MEASURE;
                            end
                        end
                        else begin
                            state <= LIN_STATE_IDLE;
                        end
                    end

                    LIN_STATE_SYNC_LOW_MEASURE:
                    begin
                        if(~break_flag)
                        begin
                            if(rxd)
                            begin
                                state <= LIN_STATE_SYNC_HIGH_MEASURE;
                            end
                            else begin
                                state <= LIN_STATE_SYNC_LOW_MEASURE;
                            end
                        end
                        else begin
                            state <= LIN_STATE_DELIMITER_DETECT;
                        end
                    end

                    LIN_STATE_CLEAR_A0:
                    begin
                        state <= LIN_STATE_SYNC_HIGH_MEASURE;
                    end

                    default:
                    begin
                        state <= LIN_STATE_IDLE;
                    end
                endcase
            end
            else begin
                state <= LIN_STATE_IDLE;
            end
        end
    end
    else begin
        always@(posedge clk_fin)
        begin
            if(ctrl_blin_start)
            begin
                case(state)
                    LIN_STATE_IDLE:
                    begin
                        if(rxd)
                        begin
                            state <= LIN_STATE_IDLE;
                        end
                        else
                        begin
                            state <= LIN_STATE_BREAK_DETECT;
                        end
                    end

                    LIN_STATE_BREAK_DETECT:
                    begin
                        if(!rxd)
                        begin
                            state <= LIN_STATE_BREAK_DETECT;
                        end
                        else begin
                            if(break_flag)
                            begin
                                state <= LIN_STATE_DELIMITER_DETECT;
                            end
                            else begin
                                state <= LIN_STATE_IDLE;
                            end
                        end
                    end

                    LIN_STATE_DELIMITER_DETECT:
                    begin
                        if(rxd)
                        begin
                            state <= LIN_STATE_DELIMITER_DETECT;
                        end
                        else begin
                            state <= LIN_STATE_IDLE;
                        end
                    end

                    default:
                    begin
                        state <= LIN_STATE_IDLE;
                    end
                endcase
            end
            else begin
                state <= LIN_STATE_IDLE;
            end
        end
    end
    endgenerate

    /* Edge Detector Implementation */
    always@(posedge clk_fin)
    begin
        rxd_reg <= rxd;
    end

    assign edge_detect = (rxd ^ rxd_reg);

    /* Break/Synch Datapath instantiation */
    cy_psoc3_dp8 #(.cy_dpconfig_a(lin_dp8_cfg))
    LINDp(
        /*  input           */ .clk(clk_fin),
        /*  input           */ .reset(1'b0),
        /*  input   [02:00] */ .cs_addr(state),
        /*  input           */ .route_si(1'b0),
        /*  input           */ .route_ci(1'b0),
        /*  input           */ .f0_load(f0_load),
        /*  input           */ .f1_load(f1_load),
        /*  input           */ .d0_load(1'b0),
        /*  input           */ .d1_load(1'b0),
        /*  output          */ .ce0(cmp_a0_d0),
        /*  output          */ .cl0(),
        /*  output          */ .z0(a0_is_zero),
        /*  output          */ .ff0(),
        /*  output          */ .ce1(),
        /*  output          */ .cl1(),
        /*  output          */ .z1(),
        /*  output          */ .ff1(),
        /*  output          */ .ov_msb(),
        /*  output          */ .co_msb(),
        /*  output          */ .cmsb(),
        /*  output          */ .so(),
        /*  output          */ .f0_bus_stat(),
        /*  output          */ .f0_blk_stat(f0_fifo_full),
        /*  output          */ .f1_bus_stat(f1_not_empty),
        /*  output          */ .f1_blk_stat()
        );

    generate
    if(BusInactivityEnabled)
    begin:InactFSM
        wire inact_cmp_d0;
        wire inact_cmp_d1;
        wire inact_ff_detect;
        reg [2:0] inact_state;
        reg inact_detect;

        /* Bus Inactivity Timer State Machine  */
        always@(posedge clk_fin)
        begin
            if(ctrl_blin_start)
            begin
                case(inact_state)
                    LIN_INACT_STATE_IDLE:
                    begin
                        inact_detect <= 1'b0;
                        if(rxd)
                        begin
                            inact_state <= LIN_INACT_STATE_INC_A0_FF_DETECT;
                        end
                        else
                        begin
                            inact_state <= LIN_INACT_STATE_IDLE;
                        end
                    end

                    LIN_INACT_STATE_INC_A0_FF_DETECT:
                    begin
                        if(rxd)
                        begin
                            if(inact_ff_detect)
                            begin
                                inact_state <= LIN_INACT_STATE_INC_A1_CMP_D1;
                            end
                            else begin
                                inact_state <= LIN_INACT_STATE_INC_A0_FF_DETECT;
                            end
                        end
                        else begin
                            inact_state <= LIN_INACT_STATE_IDLE;
                        end
                    end

                    LIN_INACT_STATE_INC_A1_CMP_D1:
                    begin
                        if(rxd)
                        begin
                            if(inact_cmp_d1)
                            begin
                                inact_state <= LIN_INACT_STATE_INC_A0_CMP_D0;
                            end
                            else begin
                                inact_state <= LIN_INACT_STATE_INC_A0_FF_DETECT;
                            end
                        end
                        else begin
                            inact_state <= LIN_INACT_STATE_IDLE;
                        end
                    end

                    LIN_INACT_STATE_INC_A0_CMP_D0:
                    begin
                        if(rxd)
                        begin
                            if(inact_cmp_d0)
                            begin
                                inact_state  <= LIN_INACT_STATE_CLEAR_A0_A1;
                            end
                            else begin
                                inact_state <= LIN_INACT_STATE_INC_A0_CMP_D0;
                            end
                        end
                        else begin
                            inact_state <= LIN_INACT_STATE_IDLE;
                        end
                    end

                    LIN_INACT_STATE_CLEAR_A0_A1:
                    begin
                        inact_state  <= LIN_INACT_STATE_IDLE;
                    end

                    default:
                    begin
                        inact_state <= LIN_INACT_STATE_IDLE;
                    end
                endcase
            end
            else begin
                inact_state <= LIN_INACT_STATE_IDLE;
            end
        end

        /* Bus Inactivity Datapath instantiation */
        cy_psoc3_dp8 #(.cy_dpconfig_a(bus_inact_dp8_cfg))
        BusInactDp(
            /*  input           */ .clk(clk_fin),
            /*  input           */ .reset(1'b0),
            /*  input   [02:00] */ .cs_addr(inact_state),
            /*  input           */ .route_si(1'b0),
            /*  input           */ .route_ci(1'b0),
            /*  input           */ .f0_load(1'b0),
            /*  input           */ .f1_load(1'b0),
            /*  input           */ .d0_load(1'b0),
            /*  input           */ .d1_load(1'b0),
            /*  output          */ .ce0(inact_cmp_d0),
            /*  output          */ .cl0(),
            /*  output          */ .z0(),
            /*  output          */ .ff0(inact_ff_detect),
            /*  output          */ .ce1(inact_cmp_d1),
            /*  output          */ .cl1(),
            /*  output          */ .z1(),
            /*  output          */ .ff1(),
            /*  output          */ .ov_msb(),
            /*  output          */ .co_msb(),
            /*  output          */ .cmsb(),
            /*  output          */ .so(),
            /*  output          */ .f0_bus_stat(),
            /*  output          */ .f0_blk_stat(),
            /*  output          */ .f1_bus_stat(),
            /*  output          */ .f1_blk_stat()
            );

    assign inact_detect_sts = inact_state[2];
    end
    endgenerate

    /* Control Register bits */
    localparam LIN_CTRL_START   = 3'd0;
    localparam LIN_CTRL_TXD_DIS = 3'd1;
    localparam LIN_CTRL_RXD_DIS = 3'd2;

    if(CY_UDB_V0)
    begin: AsyncCtl
        cy_psoc3_control #(.cy_force_order(1))
        CtrlReg(
            /* output [07:00] */  .control(control)
        );
    end
    else
    begin: SyncCtl
        cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h0), .cy_ctrl_mode_0(8'h07))
        CtrlReg(
            /*  input         */ .clock(clk_fin),
            /* output [07:00] */ .control(control)
        );
    end

    assign ctrl_blin_start = control[LIN_CTRL_START];
    assign ctrl_txd_dis    = control[LIN_CTRL_TXD_DIS];
    assign ctrl_rxd_dis    = control[LIN_CTRL_RXD_DIS];

    /* Status Register bits */
    localparam LIN_STS_BREAK_DETECT           = 3'd0;
    localparam LIN_STS_EDGE_DETECT            = 3'd1;
    localparam LIN_STS_INACT_THRESHOLD_DETECT = 3'd2;
    localparam LIN_STS_SYNC_FIELD_DETECT      = 3'd3;

    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h07), .cy_int_mask(7'h00))
    StsReg(
        /* input            */ .clock(clk_fin),
        /* input    [06:00] */ .status(status),
        /* output           */ .interrupt(interrupt)
    );

    assign status[6:4] = 3'h0;
    assign status[LIN_STS_BREAK_DETECT]           = break_pulse;
    assign status[LIN_STS_EDGE_DETECT]            = edge_detect;
    assign status[LIN_STS_INACT_THRESHOLD_DETECT] = (BusInactivityEnabled) ? inact_detect_sts : 1'b0;
    assign status[LIN_STS_SYNC_FIELD_DETECT]      = f1_not_empty;

    endmodule

    `endif /*bLIN_v1_10_V_ALREADY_INCLUDED*/

