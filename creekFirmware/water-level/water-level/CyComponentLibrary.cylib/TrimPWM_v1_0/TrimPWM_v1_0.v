/*******************************************************************************
*
* File Name: TrimPWM_v1_0.v
*
* Description:
*  The TrimPWM Component is a 8 or 16-bit PWM with dual outputs, maximum
*  period, and configurable duty cycle.
*  CompType = Less Than
*
********************************************************************************
* Data Path register definitions
********************************************************************************
* INSTANCE NAME: PWM8/PWM16
* DESCRIPTION: Implements the PWM 8-Bit U0 only; 16-bit U0 = LSB, U1 = MSB;
* REGISTER USAGE:
*  F0 => N/A
*  F1 => N/A
*  D0 => Compare value for pwm1 output
*  D1 => Compare value for pwm2 output
*  A0 => Counter (INC)
*  A1 => N/A
*
* Data Path States
*  0 0 0   Duty Cycle Increment - Inc A0
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/


`include "cypress.v"
`ifdef TrimPWM_v1_0_V_ALREADY_INCLUDED
`else
`define TrimPWM_v1_0_V_ALREADY_INCLUDED

module TrimPWM_v1_0
(
    output wire pwm1,       /* PWM 1 output     */
    output wire pwm2,       /* PWM 2 output     */
    input clock,            /* System clock     */
    input en                /* Hardware enable  */
);

    /**************************************************************************/
    /* Parameters                                                             */
    /**************************************************************************/
    /* Customizer Parameters */
    parameter   Resolution = 8'h8;      /* Resolution: 8-bit to 16-bit */

    /* Mask for MSB part of the PWM */
    localparam [7:0] dpPWM16HighAmask = 8'hFF >> (8'd16 - Resolution);


    /***************************************************************************
    *           Instantiation of udb_clock_enable primitive
    ****************************************************************************
    * The udb_clock_enable primitive component allows to support clock enable
    * mechanism and specify the intended synchronization behavior for the clock
    * result (operational clock).
    */
    wire clock_op;         /* internal clock to drive the component         */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(en),
        /* output */    .clock_out(clock_op)
    );


    /**************************************************************************
    * PWM Datapath
    **************************************************************************/
    generate
    if (Resolution <= 8)
    begin: PWM8
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0:          Duty Cycle Increment - Inc A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7:          Not Used*/
            8'hFF, 8'h00,  /*CFG9:           */
            8'hFF, 8'hFF,  /*CFG11-10:           */
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12:           cmpB=A0<D1 */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14:           */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:           */
        }
        )) PWM8dp
        (
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(clock_op),
                /*  input   [02:00]         */  .cs_addr(3'b0),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(),
                /*  output                  */  .cl0_reg(pwm1),
                /*  output                  */  .z0(),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1_reg(pwm2),
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
    else
    /* 16-bit PWM Implementation */
    begin: PWM16
        wire    nc1, nc2;    /* Unused datapath flags */

        cy_psoc3_dp16 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0:          Duty Cycle Increment - Inc A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7:          Not Used*/
            8'hFF, 8'h00,  /*CFG9:          */
            8'hFF, 8'hFF,  /*CFG11-10:          */
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12:           cmpB=A0<D1 */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_ALU, `SC_FIFO0_ALU,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14:     MSB Chain */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:          */
        }
        ), .cy_dpconfig_b(
        {
            `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0:          Duty Cycle Increment - Inc A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6:          Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7:          Not Used*/
            dpPWM16HighAmask, 8'h00,  /*CFG9:  dpPWM16HighAmask=1<<(Resolution - 9)-1*/
            8'hFF, 8'hFF,  /*CFG11-10:          */
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN,
            `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12:          Chain LSB Datapath     cmpB=A0<D1 */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_ALU, `SC_FIFO0_ALU,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
            `SC_FB_CHNED, `SC_CMP1_CHNED,
            `SC_CMP0_CHNED, /*CFG15-14:          Chain LSB Datapath*/
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:          */
        }
        )) PWM16dp
        (
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(clock_op),
                /*  input   [02:00]         */  .cs_addr(3'b0),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output  [01:00]         */  .ce0(),
                /*  output  [01:00]         */  .cl0_reg({pwm1, nc1}),
                /*  output  [01:00]         */  .z0(),
                /*  output  [01:00]         */  .ff0(),
                /*  output  [01:00]         */  .ce1(),
                /*  output  [01:00]         */  .cl1_reg({pwm2, nc2}),
                /*  output  [01:00]         */  .z1(),
                /*  output  [01:00]         */  .ff1(),
                /*  output  [01:00]         */  .ov_msb(),
                /*  output  [01:00]         */  .co_msb(),
                /*  output  [01:00]         */  .cmsb(),
                /*  output  [01:00]         */  .so(),
                /*  output  [01:00]         */  .f0_bus_stat(),
                /*  output  [01:00]         */  .f0_blk_stat(),
                /*  output  [01:00]         */  .f1_bus_stat(),
                /*  output  [01:00]         */  .f1_blk_stat()
        );
    end
    endgenerate
endmodule

`endif /* TrimPWM_v1_0_V_ALREADY_INCLUDED */
