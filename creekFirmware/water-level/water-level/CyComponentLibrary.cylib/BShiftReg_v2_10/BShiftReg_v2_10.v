/*******************************************************************************
* File Name:  BShiftReg_v2_10.v 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description: 
*  This file provides a base level model of the Shift Register componnent
*
* Note: 
*  None
********************************************************************************
*                 Control and Status Register definitions
******************************************************************************** 
*
*  Control Register Definition
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  | Desc  | unused | unused | unused | unused |unused  |unused  |unused  | CLK_EN |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *
 *      clk_en       =>   0 = Disable shift register
 *                        1 = enable shift register
 *
 *  Interrupt Status Register Definition
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *  | Desc |interrupt|f1_full|f1_n_empt|f0_n_fll|f0_empty| reset  | store  | load   |
 *  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
 *
 *    reset      =>  0 = reset event has not occured
 *                   1 = reset event has occured
 *
 *    load       =>  0 = load event has not occured
 *                   1 = load event has occured
 *
 *    store      =>  0 = store event has not occured
 *                   1 = store event has occured

 *    f0_emp     =>  0 =  F0 fifo is not empty
 *                   1 =  F0 fifo is empty
 *
 *    f0_n_full  =>  0 = F1 fifo is full
 *                   1 = F1 fifo is not full
 *
 *    f1_n_empty =>  0 = F1 is empty
 *                   1 = F1 is not empty
 *    
 *    f1_full    =>  0 = F1 is not full 
 *                   1 = F1 is full
 *
******************************************************************************** 
*                 Data Path register definitions
******************************************************************************** 
*  INSTANCE NAME:  DatapathName 
*  DESCRIPTION:
*  REGISTER USAGE:
*    F0 => input buffer
*    F1 => output buffer
*    D0 => na
*    D1 => na
*    A0 => Shift Register value 
*    A1 => Shift Register value (used for HW/SW capture)  
*
******************************************************************************** 
*               I*O Signals:   
******************************************************************************** 
*  IO SIGNALS: 
*
*	 clock        input     component clock input
*    load         input     load shift data input (A0<=F0) 
*    reset        input     component reset 
*    shiftIn      input     shift data input
*    store        input     hardware capture input (F1<=A1)
*    interrupt    output    interrupt output
*    shiftOut     output    shift data output
*
********************************************************************************
/*******************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/
`include "cypress.v"
`ifdef BShiftReg_v2_10_V_ALREADY_INCLUDED
`else
`define BShiftReg_v2_10_V_ALREADY_INCLUDED

module BShiftReg_v2_10 (
    output wire interrupt,
    output wire shiftOut,
    
	input  wire clock,
    input  wire load,
    input  wire reset,
    input  wire shiftIn,
    input  wire store

);
    /**************************************************************************/
    /* Component parameters declaration                                       */
    /**************************************************************************/
    
    parameter DefSi           = 0;
    parameter Direction       = 0;
    parameter FifoSize        = 0;
    parameter InterruptSource = 0;
    parameter Length          = 0;
    parameter UseInputFifo    = 0;
    parameter UseOutputFifo   = 0;
    parameter UseInterrupt    = 0;

    /**************************************************************************/
    /* Local parameters declaration                                           */
    /**************************************************************************/

    localparam SR_CTRL_CLK_EN        = 3'h0;    /* Clock enable */
    localparam SR_CTRL_F0_FULL       = 8'h1;    /* FIFO F0 full */

    localparam [2:0] dpMsbVal =(Length <=8)  ? (Length-6'd1):
                               (Length <=16) ? (Length-6'd8-6'd1):
                               (Length <=24) ? (Length-6'd16-6'd1):
                               (Length <=32) ? (Length-6'd24-6'd1): 6'd0;

    localparam dpDynShiftDir         = (Direction) ? `CS_SHFT_OP___SR : `CS_SHFT_OP___SL ;
    localparam dpDynShiftOut         = (Direction) ? `SC_SHIFT_SR : `SC_SHIFT_SL;
    localparam dpDynSIRouteChainA    = (Direction) ? `SC_SI_A_CHAIN : `SC_SI_A_ROUTE;
    localparam dpDynSIRouteChainB    = (Direction) ? `SC_SI_B_CHAIN : `SC_SI_B_ROUTE;
    localparam dpDynSIMSBRouteChainA = (Direction) ? `SC_SI_A_ROUTE : `SC_SI_A_CHAIN;
    localparam dpDynSIMSBRouteChainB = (Direction) ? `SC_SI_B_ROUTE : `SC_SI_B_CHAIN;
    localparam dpDynLastCmpChain     = (Direction) ? `SC_CMP1_NOCHN : `SC_CMP1_CHNED;
    localparam dpDynLastMsbCmpChain  = (Direction) ? `SC_CMP1_CHNED : `SC_CMP1_NOCHN;
    
    localparam dpDynSO16 = (Direction) ? 0 : 1;
    localparam dpDynSO24 = (Direction) ? 0 : 2;
    localparam dpDynSO32 = (Direction) ? 0 : 3;
    
    localparam SR_LOAD_EVENT        = 8'h0; 
    localparam SR_STORE_EVENT       = 8'h1; 
    localparam SR_RES_EVENT         = 8'h2; 
    localparam SR_F0_EMPTY          = 8'h3;
    localparam SR_F0_NOT_FULL       = 8'h4;
    localparam SR_F1_FULL           = 8'h5;
    localparam SR_F1_NOT_EMPTY      = 8'h6;
    
    localparam SR8     = 8'd8;
    localparam SR16    = 8'd16;
    localparam SR24    = 8'd24;
        
    /************************************************************************** 
    * UDB revisions 
    ***************************************************************************/
    localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);
    localparam CY_UDB_V1 = (!CY_UDB_V0);                         
        
    localparam dp8_cfg = {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:     Shift Operation */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:     Reset (XOR A0 A0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:     Load (A0<=F0) */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:     Reset (XOR A0 A0) */
          8'hFF, 8'h00,    /*CFG9:      */
          8'hFF, 8'hFF,    /*CFG11-10:      */
        `SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
        `SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
        `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
        `SC_SI_A_ROUTE, /*CFG13-12:      */
        `SC_A0_SRC_ACC, dpDynShiftOut, 1'h0,
        1'h0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
        `SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:      */
         3'h00, `SC_FIFO_SYNC_NONE, 6'h00,`SC_FIFO_CLK__DP,
         `SC_FIFO_CAP_FX,`SC_FIFO__EDGE,`SC_FIFO_ASYNC,
         `SC_EXTCRC_DSBL,`SC_WRK16CAT_DSBL /*CFG17-16:      */

    };

    localparam msb_dp_cfg =  {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:     Shift Operation */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:     Reset (XOR A0 A0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:     Load (A0<=F0) */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:     Reset (XOR A0 A0) */
          8'hFF, 8'h00,    /*CFG9:      */
          8'hFF, 8'hFF,    /*CFG11-10:      */
        `SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
        `SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
        `SC_A_MASK_ENBL, `SC_DEF_SI_0, dpDynSIRouteChainB,
        dpDynSIRouteChainA, /*CFG13-12:      */
        `SC_A0_SRC_ACC, dpDynShiftOut, 1'h0,
        1'h0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, dpMsbVal, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:      */
         3'h00, `SC_FIFO_SYNC_NONE, 6'h00, `SC_FIFO_CLK__DP,
         `SC_FIFO_CAP_FX,`SC_FIFO__EDGE,`SC_FIFO_ASYNC,
         `SC_EXTCRC_DSBL,`SC_WRK16CAT_DSBL /*CFG17-16:      */

    };

    localparam lsb_dp_cfg = {`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CFGRAM0:     IDLE*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CFGRAM1:     IDLE*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CFGRAM2:     IDLE*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CFGRAM3:     IDLE*/
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CFGRAM4:     Shift Operation */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CFGRAM5:     Reset (XOR A0 A0) */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CFGRAM6:     Load (A0<=F0) */
		`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CFGRAM7:     Reset (XOR A0 A0) */
		  8'hFF, 8'h00,	/*CFG9:      */
		  8'hFF, 8'hFF,	/*CFG11-10:      */
		`SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
		`SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, dpDynSIMSBRouteChainB,
		dpDynSIMSBRouteChainA, /*CFG13-12:      */
		`SC_A0_SRC_ACC, dpDynShiftOut, 1'h0,
		1'h0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
		`SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_CHNED,
		`SC_CMP0_NOCHN, /*CFG15-14:      */
		 3'h00, `SC_FIFO_SYNC_NONE, 6'h00,`SC_FIFO_CLK__DP,
         `SC_FIFO_CAP_FX,`SC_FIFO__EDGE,`SC_FIFO_ASYNC,
         `SC_EXTCRC_DSBL,`SC_WRK16CAT_DSBL /*CFG17-16:      */};

    localparam middle_dp_cfg = {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:     IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        dpDynShiftDir, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:     Shift Operation */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:     Reset (XOR A0 A0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:     Load (A0<=F0) */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:     Reset (XOR A0 A0) */
          8'hFF, 8'h00,    /*CFG9:      */
          8'hFF, 8'hFF,    /*CFG11-10:      */
        `SC_CMPB_A1_A0, `SC_CMPA_A1_A0, `SC_CI_B_REGIS,
        `SC_CI_A_REGIS, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
        `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN,
        `SC_SI_A_CHAIN, /*CFG13-12:      */
        `SC_A0_SRC_ACC, dpDynShiftOut, 1'h0,
        1'h0, `SC_FIFO1__A1, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, dpMsbVal, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_CHNED,
        `SC_CMP0_NOCHN, /*CFG15-14:      */
         3'h00, `SC_FIFO_SYNC_NONE, 6'h00,`SC_FIFO_CLK__DP,
        `SC_FIFO_CAP_FX,`SC_FIFO__EDGE,`SC_FIFO_ASYNC,
        `SC_EXTCRC_DSBL,`SC_WRK16CAT_DSBL /*CFG17-16:      */
    };

    /**************************************************************************/
    /* Wires declaration                                                      */
    /**************************************************************************/
    
    wire f0_blk_stat_8;
    wire f0_bus_stat_8;
    wire f1_blk_stat_8;
    wire f1_bus_stat_8;

    wire [1:0] f0_blk_stat_16;
    wire [1:0] f0_bus_stat_16;
    wire [1:0] f1_blk_stat_16;
    wire [1:0] f1_bus_stat_16;

    wire [2:0] f0_blk_stat_24;
    wire [2:0] f0_bus_stat_24;
    wire [2:0] f1_blk_stat_24;
    wire [2:0] f1_bus_stat_24;

    wire [3:0] f0_blk_stat_32;
    wire [3:0] f0_bus_stat_32;
    wire [3:0] f1_blk_stat_32;
    wire [3:0] f1_bus_stat_32;

    wire final_load;
    reg load_reg;   
    wire    [7:0]    control; /* Control Register Output */

    wire    ctrl_clk_enable   = control[SR_CTRL_CLK_EN];
    wire    ctrl_f0_full      = control[SR_CTRL_F0_FULL];

    wire f0_bus_stat_final;
    wire f0_blk_stat_final;
    wire f1_blk_stat_final;
    wire f1_bus_stat_final;
    
    
    /*Shift out wires of all Datapaths (8, 16, 24, 32 bits width) */

    wire so_8;
    wire [1:0] so_16;
    wire [2:0] so_24;
    wire [3:0] so_32;

    wire    [6:0]    status;    /* Status Register Input */
    
    wire clk_fin;
        
    /**************************************************************************/
    /* Clock Enable for the component */
    /**************************************************************************/
       
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    ClkEn (
        
        .clock_in(clock),
        .enable(1'b1),
        .clock_out(clk_fin)
    
    );
        
    /**************************************************************************/
    /* Control Register Implementation                                        */
    /**************************************************************************/
   
    /* Instantiation of control register */
    
    generate
    
        if(CY_UDB_V0)
        begin: AsyncCtl
        
            cy_psoc3_control #(.cy_force_order(1)) 
            CtrlReg (
                /* output [07:00] */  .control(control)
            );
        
        end /* AsyncCtl */

        else
        begin: SyncCtl

            cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h0), .cy_ctrl_mode_0(8'h01)) 
            CtrlReg (
                    /*  input         */  .clock(clk_fin),
                    /* output [07:00] */  .control(control)
            );

        end /* SyncCtl */

    endgenerate
    
    /**************************************************************************/
    /* Status Register Implementation                                         */
    /**************************************************************************/

    assign status[SR_RES_EVENT]    = reset;            
    assign status[SR_LOAD_EVENT]   = final_load;       
    assign status[SR_STORE_EVENT]  = store;            
    assign status[SR_F0_EMPTY]     = f0_blk_stat_final;
    assign status[SR_F0_NOT_FULL]  = f0_bus_stat_final;
    assign status[SR_F1_FULL]      = f1_blk_stat_final;
    assign status[SR_F1_NOT_EMPTY] = f1_bus_stat_final;

    /* Instantiation of status register*/
    
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h07),
                       .cy_int_mask(7'h07))
    StsReg(
        /* input          */ .clock(clk_fin),
        /* input  [06:00] */ .status(status),
        /* output         */ .interrupt(interrupt)
    );

    always@(posedge clk_fin)
    begin
        load_reg <= load; 
    end
    
    assign final_load = ~load_reg & load;
    
    generate

      /*8-bit shift register implementation
      shift input is configured as 'routed'*/

    if(Length <= SR8) begin:sC8

        assign f0_blk_stat_final = f0_blk_stat_8;
        assign f0_bus_stat_final = f0_bus_stat_8;
        assign f1_blk_stat_final = f1_blk_stat_8;
        assign f1_bus_stat_final = f1_bus_stat_8;

        cy_psoc3_dp8 #(.cy_dpconfig_a(dp8_cfg))
        BShiftRegDp(
                /* input         */ .clk(clk_fin),                
                /* input [02:00] */ .cs_addr({ctrl_clk_enable,final_load,reset}),
                /* input         */ .route_si(shiftIn),         
                /* input         */ .route_ci(1'b0),              
                /* input         */ .f0_load(1'b0),               
                /* input         */ .f1_load(store),      
                /* input         */ .d0_load(1'b0),               
                /* input         */ .d1_load(1'b0),               
                /* output        */ .ce0(),                      
                /* output        */ .cl0(),                      
                /* output        */ .z0(),                       
                /* output        */ .ff0(),                      
                /* output        */ .ce1(),                      
                /* output        */ .cl1(),                      
                /* output        */ .z1(),                       
                /* output        */ .ff1(),                      
                /* output        */ .ov_msb(),                   
                /* output        */ .co_msb(),                   
                /* output        */ .cmsb(),                     
                /* output        */ .so(so_8),                   
                /* output        */ .f0_bus_stat(f0_bus_stat_8), 
                /* output        */ .f0_blk_stat(f0_blk_stat_8), 
                /* output        */ .f1_bus_stat(f1_bus_stat_8), 
                /* output        */ .f1_blk_stat(f1_blk_stat_8)  
                );

        /*Driving component 'shift_out' output*/

        assign shiftOut=so_8;

    end /*End of entire 8 bit section*/

    /* 16-bit shift register implementation */

    else if(Length <= SR16) begin:sC16

        assign f0_blk_stat_final=f0_blk_stat_16[1];
        assign f0_bus_stat_final=f0_bus_stat_16[1];
        assign f1_blk_stat_final=f1_blk_stat_16[1];
        assign f1_bus_stat_final=f1_bus_stat_16[1];

        cy_psoc3_dp16 #(.cy_dpconfig_a(msb_dp_cfg), .cy_dpconfig_b(lsb_dp_cfg))
        BShiftRegDp(
            /* input          */ .clk(clk_fin),                           
            /* input [02:00]  */ .cs_addr({ctrl_clk_enable, final_load, reset}), 
            /* input          */ .route_si(shiftIn),                    
            /* input          */ .route_ci(1'b0),                       
            /* input          */ .f0_load(1'b0),                        
            /* input          */ .f1_load(store),                 
            /* input          */ .d0_load(1'b0),                        
            /* input          */ .d1_load(1'b0),                        
            /* output [01:00] */ .ce0(),                       
            /* output [01:00] */ .cl0(),                       
            /* output [01:00] */ .z0(),                        
            /* output [01:00] */ .ff0(),                       
            /* output [01:00] */ .ce1(),                       
            /* output [01:00] */ .cl1(),                       
            /* output [01:00] */ .z1(),                        
            /* output [01:00] */ .ff1(),                       
            /* output [01:00] */ .ov_msb(),                    
            /* output [01:00] */ .co_msb(),                    
            /* output [01:00] */ .cmsb(),                      
            /* output [01:00] */ .so(so_16),                   
            /* output [01:00] */ .f0_bus_stat(f0_bus_stat_16), 
            /* output [01:00] */ .f0_blk_stat(f0_blk_stat_16), 
            /* output [01:00] */ .f1_bus_stat(f1_bus_stat_16), 
            /* output [01:00] */ .f1_blk_stat(f1_blk_stat_16)  
        );

            /*Driving component 'shift_out' output*/

        assign shiftOut=so_16[dpDynSO16];

    end /*End of entire 16 bit section*/

    /* 24-bit shift register implementation*/

    else if(Length <= SR24) begin : sC24

        assign f0_blk_stat_final = f0_blk_stat_24[2];
        assign f0_bus_stat_final = f0_bus_stat_24[2];
        assign f1_blk_stat_final = f1_blk_stat_24[2];
        assign f1_bus_stat_final = f1_bus_stat_24[2];

        cy_psoc3_dp24 #(.cy_dpconfig_a(msb_dp_cfg), .cy_dpconfig_b(middle_dp_cfg), 
                        .cy_dpconfig_c(lsb_dp_cfg))
        BShiftRegDp(
            /* input          */ .clk(clk_fin),                           
            /* input [02:00]  */ .cs_addr({ctrl_clk_enable, final_load, reset}), 
            /* input          */ .route_si(shiftIn),                    
            /* input          */ .route_ci(1'b0),                       
            /* input          */ .f0_load(1'b0),                        
            /* input          */ .f1_load(store),                 
            /* input          */ .d0_load(1'b0),                        
            /* input          */ .d1_load(1'b0),                        
            /* output [02:00] */ .ce0(),                       
            /* output [02:00] */ .cl0(),                       
            /* output [02:00] */ .z0(),                        
            /* output [02:00] */ .ff0(),                       
            /* output [02:00] */ .ce1(),                       
            /* output [02:00] */ .cl1(),                       
            /* output [02:00] */ .z1(),                        
            /* output [02:00] */ .ff1(),                       
            /* output [02:00] */ .ov_msb(),                    
            /* output [02:00] */ .co_msb(),                    
            /* output [02:00] */ .cmsb(),                      
            /* output [02:00] */ .so(so_24),                   
            /* output [02:00] */ .f0_bus_stat(f0_bus_stat_24), 
            /* output [02:00] */ .f0_blk_stat(f0_blk_stat_24), 
            /* output [02:00] */ .f1_bus_stat(f1_bus_stat_24),
            /* output [02:00] */ .f1_blk_stat(f1_blk_stat_24)  
        );

            /*Driving component 'shift_out' output*/
            assign shiftOut=so_24[dpDynSO24];

        end /*End of entire 24 bit section*/

    /* 32-bit shift register implementation */

    else begin : sC32
        assign f0_blk_stat_final=f0_blk_stat_32[3];
        assign f0_bus_stat_final=f0_bus_stat_32[3];
        assign f1_blk_stat_final=f1_blk_stat_32[3];
        assign f1_bus_stat_final=f1_bus_stat_32[3];

        cy_psoc3_dp32 #(.cy_dpconfig_a(msb_dp_cfg), .cy_dpconfig_b(middle_dp_cfg),
                        .cy_dpconfig_c(middle_dp_cfg), .cy_dpconfig_d(lsb_dp_cfg))
        BShiftRegDp(
            /* input          */ .clk(clk_fin),                           
            /* input [02:00]  */ .cs_addr({ctrl_clk_enable, final_load, reset}), 
            /* input          */ .route_si(shiftIn),                   
            /* input          */ .route_ci(1'b0),                       
            /* input          */ .f0_load(1'b0),                        
            /* input          */ .f1_load(store),                 
            /* input          */ .d0_load(1'b0),               
            /* input          */ .d1_load(1'b0),               
            /* output [03:00] */ .ce0(),                       
            /* output [03:00] */ .cl0(),                       
            /* output [03:00] */ .z0(),                        
            /* output [03:00] */ .ff0(),                       
            /* output [03:00] */ .ce1(),                       
            /* output [03:00] */ .cl1(),                       
            /* output [03:00] */ .z1(),                        
            /* output [03:00] */ .ff1(),                       
            /* output [03:00] */ .ov_msb(),                    
            /* output [03:00] */ .co_msb(),                    
            /* output [03:00] */ .cmsb(),                      
            /* output [03:00] */ .so(so_32),                   
            /* output [03:00] */ .f0_bus_stat(f0_bus_stat_32), 
            /* output [03:00] */ .f0_blk_stat(f0_blk_stat_32), 
            /* output [03:00] */ .f1_bus_stat(f1_bus_stat_32), 
            /* output [03:00] */ .f1_blk_stat(f1_blk_stat_32)  
        );

            /*Driving component 'shift_out' output*/

        assign shiftOut=so_32[dpDynSO32];

    end /*End of entire 32 bit section*/

    endgenerate

endmodule
`endif      /* BShiftReg_v2_10_V_ALREADY_INCLUDED */
