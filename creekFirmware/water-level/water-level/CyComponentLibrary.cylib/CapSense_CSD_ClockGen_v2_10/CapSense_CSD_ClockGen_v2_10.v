/*******************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef CapSense_CSD_ClockGen_v2_10_V_ALREADY_INCLUDED
`else
`define CapSense_CSD_ClockGen_v2_10_V_ALREADY_INCLUDED

module CapSense_CSD_ClockGen_v2_10 
(
    input    wire   clock,
    input    wire   reset,
    input    wire   enable,
    output   wire   dpulse,
    output   wire   ppulse,
    output   wire   start0,
    output   wire   start1,
    output   wire   mesrst,
    output   wire   shield
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
    
    /* PRS State Machine States */
    localparam PRS_STATE_CALC_LOWER = 2'd0;  /* Calculate Lower Half */
    localparam PRS_STATE_SAVE_LOWER = 2'd1;  /* Save Lower Half */
    localparam PRS_STATE_CALC_UPPER = 2'd2;  /* Calculate Upper Half */
    localparam PRS_STATE_SAVE_UPPER = 2'd3;  /* Save Upper Half */

    /* CloclGen State Machine States */
    localparam CAPSNS_CLK_GEN_IDLE  = 3'd0;  
    localparam CAPSNS_CLK_GEN_S1    = 3'd1;
    localparam CAPSNS_CLK_GEN_S2    = 3'd2;
    localparam CAPSNS_CLK_GEN_S3    = 3'd4;

    /* Others parameters */
    localparam CAPSNS_CLK_GEN_PRESCALER_NONE = 2'd0;
    localparam CAPSNS_CLK_GEN_PRESCALER_UDB  = 2'd1;
    localparam CAPSNS_CLK_GEN_PRESCALER_FF   = 2'd2;
    parameter PrescalerOptions = CAPSNS_CLK_GEN_PRESCALER_UDB;
    
    localparam CAPSNS_CLK_GEN_IDAC_OPTIONS_NONE     = 2'd0;
    localparam CAPSNS_CLK_GEN_IDAC_OPTIONS_SOURCE   = 2'd1;
    localparam CAPSNS_CLK_GEN_IDAC_OPTIONS_SINK     = 2'd2;
    parameter IdacOptions = CAPSNS_CLK_GEN_IDAC_OPTIONS_SOURCE;
    
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
    
    localparam dpconfig_prescaler =
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
    };

    localparam dpconfig_prs8 =
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
    };

    localparam dpconfig_prs16a =
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
    };

    localparam dpconfig_prs16b =
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
    };
    
    /* Signals */
    wire inter_reset;
    wire tmp_dpulse;
    wire tmp_pclk;
    wire work_en;
    wire bitstream;

    reg [2:0] cstate;
    
    /* Control Register */
    wire   syncen;
    wire   mesen;
    wire   ch0en;
    wire   ch1en;
    wire [7:0] control; /* Control Register Output */
    
    /* Prescaler */

    wire tmp_ppulse_ff;
    wire tmp_ppulse_udb;
    wire ppulse_less;
    wire ppulse_equal;
    wire prescaler;
    wire tmp_ppulse;
    wire clock_detect;
    wire z0;
    wire [2:0] cs_addr;

    reg tmp_ppulse_dly;
    reg clock_detect_reg;
    reg tmp_ppulse_reg;

    /* PRS */
    wire so;
    wire ci;
    wire si;
    wire cmsb;
    wire cmsb_reg;
    wire save;
    wire dcfg;
    wire si_a;
    wire so_a;
    wire resetx_all;
    wire [7:0] sc_out_a;
    wire [7:0] sc_out;
    wire [2:0] prs_cs_addr;
    
    reg so_reg;
    reg ci_temp;
    reg sc_temp;
    reg resetx;
    reg resetx_reg;
    reg [1:0] state;

      
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
        /* input        */ .reset(inter_reset),
        /* input        */ .load(1'b0),
        /* input        */ .enable(1'b1), /* TODO: Need think about this*/
        /* output [6:0] */ .count(),
        /* output       */ .tc(tmp_dpulse)
    );

    /* Prescaler Instantiation */
    generate
        if(PrescalerOptions == CAPSNS_CLK_GEN_PRESCALER_FF)
        begin: FF
            cy_psoc3_timer_v1_0 Prescaler
            (
                /* input */     .clock(clock), /* Use clock because there is not clock_enable componnent for FF*/
                /* input */     .kill(1'b0),
                /* input */     .enable(1'b1),
                /* input */     .capture(1'b0),
                /* input */     .timer_reset(inter_reset),
                /* output */    .tc(),
                /* output */    .compare(tmp_ppulse_ff),
                /* output */    .interrupt()
            );
        end
        else if(PrescalerOptions == CAPSNS_CLK_GEN_PRESCALER_UDB)
        begin: UDB
            cy_psoc3_dp8 #(.a0_init_a(8'hFF),.cy_dpconfig_a(dpconfig_prescaler)) 
            PrescalerDp(
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
        end
    endgenerate
   
    /* PRS Instantiation */
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
        cy_psoc3_dp8 #(.cy_dpconfig_a(dpconfig_prs8)) 
        PRSdp(
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
        /*  output                  */  .cmsb_reg(cmsb_reg),
        /*  output                  */  .so(),
        /*  output                  */  .f0_bus_stat(),
        /*  output                  */  .f0_blk_stat(),
        /*  output                  */  .f1_bus_stat(),
        /*  output                  */  .f1_blk_stat()
        );
        end 
        else if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS) 
        begin : sC16
        cy_psoc3_dp16 #(.cy_dpconfig_a(dpconfig_prs16a),.cy_dpconfig_b(dpconfig_prs16b)) 
        PRSdp(
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
        /* output [01:00]   */ .cmsb_reg({cmsb_reg, nc1}),
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
    
    /* Prescaler procedures */
    generate
        if (PrescalerOptions != CAPSNS_CLK_GEN_PRESCALER_NONE)
        begin
            if(PrescalerOptions == CAPSNS_CLK_GEN_PRESCALER_FF)
            begin
                always @(posedge op_clock) tmp_ppulse_reg <= tmp_ppulse_ff;
            end
            else if(PrescalerOptions == CAPSNS_CLK_GEN_PRESCALER_UDB)
            begin
                always @(posedge op_clock) tmp_ppulse_reg <= tmp_ppulse_udb;
            end
            always @(posedge op_clock) tmp_ppulse_dly <= tmp_ppulse;
            always @(posedge op_clock) clock_detect_reg <= clock_detect;
        end
        else 
        begin
            always @(posedge op_clock) clock_detect_reg <= clock_detect;
        end
    endgenerate
    
    
    /* PRS with Time Multiplexing Logic procedures */
    generate
        if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS_TDM)
        begin
            
            always @(posedge clk_TDM)
            begin
                if (resetx_reg)
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
            
            always @(posedge clk_TDM)
            begin
                if (resetx_reg)
                begin
                    ci_temp = 1'b1;
                    sc_temp = 1'b1;
                    so_reg  = 1'b1;
                end
                else if (save)
                begin
                    ci_temp = ci;    /* ci */
                    sc_temp = sc_out[dpPOVal];    /* si */
                end
                else so_reg = so;    /* sync so */
            end 
            
            /* Reset forming */
            always @(posedge clk_TDM or posedge inter_reset)
            begin
                if (inter_reset) resetx<=1'b1; 
                else resetx<=1'b0;
            end
            
            always @(posedge op_clock) 
            begin
                resetx_reg <=resetx;
            end
        end
    endgenerate

    /* Others procedures */
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
    
    /* Prescaler procedures */
    generate
        if (PrescalerOptions == CAPSNS_CLK_GEN_PRESCALER_NONE)
        begin
            assign clock_detect = 1'b1;
            assign prescaler = clock;
        end
        else 
        begin
            if(PrescalerOptions == CAPSNS_CLK_GEN_PRESCALER_UDB)
            begin
                assign tmp_ppulse_udb=ppulse_equal | ppulse_less;
            end
            assign clock_detect = tmp_ppulse & ~tmp_ppulse_dly;
            assign tmp_ppulse = tmp_ppulse_reg;
            assign prescaler = tmp_ppulse;
            assign cs_addr = {1'b0, z0, inter_reset};
        end
    endgenerate
        
    /* PRS procedures */
    generate
        if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_16BITS_TDM)
        begin
            assign so = so_a;
            assign sc_out = sc_out_a;
            assign ci = dcfg ? sc_out[dpPOVal] : ci_temp;
            assign si = dcfg ? so_reg : sc_temp;

            assign save = ((state == PRS_STATE_SAVE_LOWER) | (state == PRS_STATE_SAVE_UPPER));
            assign dcfg = ((state == PRS_STATE_CALC_UPPER) | (state == PRS_STATE_SAVE_UPPER));
            
            assign prs_cs_addr = {resetx_reg, state[1:0]};
            assign bitstream = ci_temp;
        end
        else if (PrsOptions != CAPSNS_CLK_GEN_PRS_OPTIONS_NONE)
        begin
            assign prs_cs_addr = {inter_reset,1'b0, clock_detect_reg};
            assign bitstream = cmsb_reg;
        end
    endgenerate
    
    /* Others procedures */
    generate
        if (PrsOptions == CAPSNS_CLK_GEN_PRS_OPTIONS_NONE)
        begin
            assign tmp_pclk = prescaler;
        end
        else
        begin
            assign tmp_pclk = bitstream;
        end
    endgenerate    

    generate
        if (IdacOptions==CAPSNS_CLK_GEN_IDAC_OPTIONS_SOURCE)
        begin
            assign shield = tmp_pclk;
        end
        else
        begin
            assign shield = ~tmp_pclk;
        end
    endgenerate
    
    assign dpulse = tmp_dpulse;  /* digatal clock */
    assign ppulse = tmp_pclk;    /* precharge clock*/
    
    assign inter_reset  = cstate[0];
    assign mesrst       = cstate[1];
    assign work_en      = cstate[2];
    
    assign start0 = work_en & ch0en;
    assign start1 = work_en & ch1en;
    
    assign syncen   = control[0];
    assign mesen    = control[1];
    assign ch0en    = control[2];
    assign ch1en    = control[3];

endmodule

`endif /* CapSense_CSD_ClockGen_v2_10_V ALREADY_INCLUDED */
