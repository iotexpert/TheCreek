/*******************************************************************************
* File Name:  PRS_v2_10.v 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description: 
*  This file provides a top level model of the PRS componnent
*  defining and all of the necessary interconnect.
* 
* Note: 
*  None
********************************************************************************
*                 Control and Status Register definitions
******************************************************************************** 
*
* Control Register Definition
*
*  ctrl_enable => 0 = disable PRS
*                 1 = enable PRS
*
*  api_clock   => Software clock for PRS. Firstly must be set "1", 
*                 after that - "0". This is emulate rising edge of clock signal.
*
*  dff_reset   => Software reset for dtrig. Firstly must be set "1", 
*                 after that - "0". This is emulate rising edge for reset 
*                 triggers.
*
******************************************************************************** 
*                 Data Path register definitions
******************************************************************************** 
*  INSTANCE NAME:  DatapathName 
*  DESCRIPTION:
*  REGISTER USAGE:
*    F0 => na
*    F1 => na
*    D0 => Lower half of polynomial value
*    D1 => Upper half of polynomial value
*    A0 => Lower half of seed value 
*    A1 => Upper half of seed value  
*
******************************************************************************** 
*               I*O Signals:   
******************************************************************************** 
*  IO SIGNALS: 
*   clock         input        Data clock
*	enable		  input		   Enable 
*	reset		  input 	   Reset
*   bitstream     output       Bitstream
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef PRS_v2_10_V_ALREADY_INCLUDED
`else
`define PRS_v2_10_V_ALREADY_INCLUDED

module PRS_v2_10
(
    input wire clock,
    input wire enable,
    input wire reset,
    output wire bitstream
);

    /**************************************************************************/
    /* Parameters                                                             */
    /**************************************************************************/
    localparam PRS_8_BIT  = 7'd8;
    localparam PRS_16_BIT = 7'd16;
    localparam PRS_24_BIT = 7'd24;
    localparam PRS_32_BIT = 7'd32;
    localparam PRS_40_BIT = 7'd40;
    localparam PRS_48_BIT = 7'd48;
    localparam PRS_56_BIT = 7'd56;
    localparam PRS_64_BIT = 7'd64;
    parameter [6:0] Resolution = PRS_8_BIT;    
   
    localparam PRS_RUNMODE_CLOCKED         = 1'd0;
    localparam PRS_RUNMODE_API_SINGLE_STEP = 1'd1;
    parameter RunMode = PRS_RUNMODE_CLOCKED;
   
    localparam PRS_CFG_TDM_DISABLE = 1'd0;
    localparam PRS_CFG_TDM_ENABLE  = 1'd1;
    parameter TimeMultiplexing = PRS_CFG_TDM_DISABLE;
    
    localparam PRS_WAKEUP_BEHAVIOUR_START_AFRESH = 1'd0;
    localparam PRS_WAKEUP_BEHAVIOUR_RESUME_WORK  = 1'd1;
    parameter WakeupBehaviour = PRS_WAKEUP_BEHAVIOUR_START_AFRESH;
  
    /* UDB Revision definitions */
    localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);
    localparam CY_UDB_V1 = (!CY_UDB_V0);
    
    /* Control Register Bits (Bits 7-3 are unused )*/
    localparam PRS_CTRL_ENABLE          = 3'h0;   /* Enable PRS */             
    localparam PRS_CTRL_RISING_EDGE     = 3'h1;   /* API single step */
    localparam PRS_CTRL_RESET_COMMON    = 3'h2;   /* Software reset for triggers */ 
    localparam PRS_CTRL_RESET_CI        = 3'h3;   
    localparam PRS_CTRL_RESET_SI        = 3'h4;   
    localparam PRS_CTRL_RESET_SO        = 3'h5;   
    localparam PRS_CTRL_RESET_STATE0    = 3'h6;   
    localparam PRS_CTRL_RESET_STATE1    = 3'h7;   
    
    /* State Machine States */
    localparam PRS_STATE_CALC_LOWER = 2'd0;  /* Calculate Lower Half */
    localparam PRS_STATE_SAVE_LOWER = 2'd1;  /* Save Lower Half */
    localparam PRS_STATE_CALC_UPPER = 2'd2;  /* Calculate Upper Half */
    localparam PRS_STATE_SAVE_UPPER = 2'd3;  /* Save Upper Half */
 
    
   
    localparam [2:0] dpPOVal = (Resolution <= PRS_8_BIT)  ? (Resolution - 1): 
                               (Resolution <= PRS_16_BIT) ? (Resolution - 9):
                               (Resolution <= PRS_24_BIT) ? (Resolution - 17):
                               (Resolution <= PRS_32_BIT) ? (Resolution - 25):
                               (Resolution <= PRS_40_BIT) ? (Resolution - 33):
                               (Resolution <= PRS_48_BIT) ? (Resolution - 41):
                               (Resolution <= PRS_56_BIT) ? (Resolution - 49):
                               (Resolution <= PRS_64_BIT) ? (Resolution - 57): 0;

    localparam [2:0] dpMsbVal = (Resolution < PRS_8_BIT) ? (Resolution - 1) : 7;
    
    localparam dpconfig0 =    
    {
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Calculate Lower Half */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Save Lower Half */
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
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
    
    /* Internal signals */
    wire clk;
    wire so;
    
    wire ci;
    wire si;
    wire cmsb;
    wire si_final;
    wire so_chain;
    wire si_d;
    wire si_c;
    wire si_b;
    wire si_a;
    wire so_d;
    wire so_c;
    wire so_b;
    wire so_a;
    wire [7:0] sc_out_a;
    wire [7:0] sc_out_b;
    wire [7:0] sc_out_c;
    wire [7:0] sc_out_d;
    wire [7:0] sc_out;
    wire [2:0] cs_addr;   

    wire enable_final;
    wire reset_final;
    wire ctrl_enable;
    wire ctrl_api_clock;
    wire ctrl_dff_reset;
    wire ctrl_reset_ci;
    wire ctrl_reset_si;
    wire ctrl_reset_so;
    wire ctrl_reset_common;
    wire [1:0] ctrl_reset_state;
    wire ctrl_api_clock_pulse;

    wire save_so;
    wire save; 
    wire dcfg;
    wire clk_ctrl;
    wire so_reg1;
    
    wire [7:0] control; /* Control Register Output */    
    wire [7:0] status;  /* Status Register Input */    
    
    reg so_reg;
    reg ci_temp;
    reg sc_temp;
    reg [1:0] state;

    wire reset_ci ;
    wire reset_si;
    wire reset_so;
    wire reset_state0;
    wire reset_state1;      
    
    /**************************************************************************/
    /* Hierarchy - instantiating another module                               */
    /**************************************************************************/
    generate
        if ((TimeMultiplexing == PRS_CFG_TDM_ENABLE) && (WakeupBehaviour == PRS_WAKEUP_BEHAVIOUR_RESUME_WORK)) 
        begin : Sts 
            cy_psoc3_status #(.cy_force_order(`TRUE), .cy_md_select(8'h00)) 
            StsReg (
                /* input          */ .clock(clk_ctrl),
                /* input  [07:00] */ .status(status)
            );
        end
    endgenerate
    
    
    generate
        if (RunMode == PRS_RUNMODE_CLOCKED)
        begin
            cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
            Sync1(
                /* input  */.clock_in(clock),
                /* input  */.enable(enable_final),
                /* output */.clock_out(clk)
            );
            cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
            Sync2(
                /* input  */.clock_in(clock),
                /* input  */.enable(1'b1),
                /* output */.clock_out(clk_ctrl)
            );
        end
        else
        begin
            cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
            Sync1(
                /* input  */.clock_in(ctrl_api_clock),
                /* input  */.enable(enable_final),
                /* output */.clock_out(clk)
            );
            cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
            Sync2(
                /* input  */.clock_in(ctrl_api_clock),
                /* input  */.enable(1'b1),
                /* output */.clock_out(clk_ctrl)
            );
        end
    endgenerate
    
    generate
        if (CY_UDB_V0)
        begin: AsyncCtrl
            cy_psoc3_control #(.cy_force_order(1)) 
            CtrlReg (
                /* output 07:00] */.control(control)
            );
        end
        else
        begin: SyncCtrl
            if (RunMode == PRS_RUNMODE_CLOCKED)            
            begin: ClkSp
                cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h06), .cy_ctrl_mode_0(8'hFF)) 
                CtrlReg (
                    /* input          */ .clock(clk_ctrl),
                    /* output [07:00] */ .control(control)
                );
            end
            else
            begin: ClkSp
                cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'h00)) 
                CtrlReg (
                    /* output [07:00] */ .control(control)
                );
            end
        end
    endgenerate

    generate
        if ((Resolution > PRS_48_BIT) && (TimeMultiplexing == PRS_CFG_TDM_ENABLE))
        begin : b3 
            cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
            PRSdp_d(
                /*  input                   */  .clk(clk),        
                /*  input   [02:00]         */  .cs_addr(cs_addr),
                /*  input                   */  .route_si(si_d),      
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
                /*  output                  */  .cmsb(), 
                /*  output                  */  .so(so_d),  
                /*  output                  */  .f0_bus_stat(),     
                /*  output                  */  .f0_blk_stat(),     
                /*  output                  */  .f1_bus_stat(),     
                /*  output                  */  .f1_blk_stat(),
        
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
                /* output [07:00]           */  .po(sc_out_d[7:0])         
            );
        end 
        if ((Resolution > PRS_32_BIT) && (TimeMultiplexing == PRS_CFG_TDM_ENABLE))
        begin : b2
            cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
            PRSdp_c(
                /*  input                   */  .clk(clk),     
                /* input          */ .reset(1'b0),                
                /* input   [02:00]         */  .cs_addr(cs_addr),
                /* input                   */  .route_si(si_c),      
                /* input                   */  .route_ci(ci),    
                /* input                   */  .f0_load(1'b0),     
                /* input                   */  .f1_load(1'b0),     
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
                /*  output                  */  .cmsb(), 
                /*  output                  */  .so(so_c),  
                /*  output                  */  .f0_bus_stat(),     
                /*  output                  */  .f0_blk_stat(),     
                /*  output                  */  .f1_bus_stat(),     
                /*  output                  */  .f1_blk_stat(),
        
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
                /* output [07:00]           */  .po(sc_out_c[7:0])          
            );            
        end 
        if ((Resolution > PRS_16_BIT) && (TimeMultiplexing == PRS_CFG_TDM_ENABLE))
        begin : b1
            cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
            PRSdp_b(
                /* input                   */  .clk(clk),        
                /* input   [02:00]         */  .cs_addr(cs_addr),
                /* input                   */  .route_si(si_b),      
                /* input                   */  .route_ci(ci),    
                /* input                   */  .f0_load(1'b0),     
                /* input                   */  .f1_load(1'b0),     
                /* input                   */  .d0_load(1'b0),     
                /* input                   */  .d1_load(1'b0),     
                /* output                  */  .ce0(),             
                /* output                  */  .cl0(),             
                /* output                  */  .z0(),              
                /* output                  */  .ff0(),             
                /* output                  */  .ce1(),
                /* output                  */  .cl1(),
                /* output                  */  .z1(),
                /* output                  */  .ff1(),
                /* output                  */  .ov_msb(),
                /* output                  */  .co_msb(), 
                /* output                  */  .cmsb(), 
                /* output                  */  .so(so_b),  
                /* output                  */  .f0_bus_stat(),     
                /* output                  */  .f0_blk_stat(),     
                /* output                  */  .f1_bus_stat(),     
                /* output                  */  .f1_blk_stat(),
        
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
                /* output [07:00]           */  .po(sc_out_b[7:0])          
            );
        end 
        if ((Resolution > PRS_8_BIT) && (TimeMultiplexing == PRS_CFG_TDM_ENABLE))
        begin : b0
            cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) 
            PRSdp_a(
             /*  input                   */  .clk(clk),        
             /*  input   [02:00]         */  .cs_addr(cs_addr),
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
    endgenerate
   
   
    generate
        if ((Resolution <= PRS_8_BIT) && (TimeMultiplexing == PRS_CFG_TDM_DISABLE))
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
	 	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
	 	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
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
        /*  input                   */  .clk(clk),    
        /* input          */ .reset(1'b0),           
        /*  input   [02:00]         */  .cs_addr(cs_addr),
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
        else if ((Resolution <= PRS_16_BIT) && (TimeMultiplexing == PRS_CFG_TDM_DISABLE)) 
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
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
        /* input            */ .clk(clk),                 
        /* input  [02:00]   */ .cs_addr(cs_addr),              
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
        else if ((Resolution <= PRS_24_BIT) && (TimeMultiplexing == PRS_CFG_TDM_DISABLE))
        begin : sC24
        cy_psoc3_dp24 #(.cy_dpconfig_a(
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        }), .cy_dpconfig_c(
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpPOVal, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        })) PRSdp(
        /* input            */ .clk(clk),          
        /* input  [02:00]   */ .cs_addr(cs_addr),  
        /* input            */ .route_si(1'b0),    
        /* input            */ .route_ci(1'b0),  
        /* input            */ .f0_load(1'b0), 
        /* input            */ .f1_load(1'b0),  
        /* input            */ .d0_load(1'b0),   
        /* input            */ .d1_load(1'b0),  
        /* output [02:00]   */ .ce0(),         
        /* output [02:00]   */ .cl0(),        
        /* output [02:00]   */ .z0(),       
        /* output [02:00]   */ .ff0(),    
        /* output [02:00]   */ .ce1(), 
        /* output [02:00]   */ .cl1(),  
        /* output [02:00]   */ .z1(),   
        /* output [02:00]   */ .ff1(), 
        /* output [02:00]   */ .ov_msb(),  
        /* output [02:00]   */ .co_msb(),  
        /* output [02:00]   */ .cmsb({cmsb, nc1, nc2}),   
        /* output [02:00]   */ .so(),     
        /* output [02:00]   */ .f0_bus_stat(), 
        /* output [02:00]   */ .f0_blk_stat(),  
        /* output [02:00]   */ .f1_bus_stat(),  
        /* output [02:00]   */ .f1_blk_stat()   
        );
        end 
        else if ((Resolution <= PRS_32_BIT) && (TimeMultiplexing == PRS_CFG_TDM_DISABLE)) 
        begin : sC32
        cy_psoc3_dp32 #(.cy_dpconfig_a(
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        }), .cy_dpconfig_c(
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_CHNED,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        }), .cy_dpconfig_d(
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
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_CHAIN, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    	`SC_MSB_ENBL, dpPOVal, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        })) PRSdp(
        /* input            */ .clk(clk),          
        /* input  [02:00]   */ .cs_addr(cs_addr),   
        /* input            */ .route_si(1'b0),     
        /* input            */ .route_ci(1'b0),    
        /* input            */ .f0_load(1'b0),     
        /* input            */ .f1_load(1'b0),     
        /* input            */ .d0_load(1'b0),    
        /* input            */ .d1_load(1'b0),   
        /* output [03:00]   */ .ce0(),         
        /* output [03:00]   */ .cl0(),        
        /* output [03:00]   */ .z0(),        
        /* output [03:00]   */ .ff0(),       
        /* output [03:00]   */ .ce1(),        
        /* output [03:00]   */ .cl1(),       
        /* output [03:00]   */ .z1(),       
        /* output [03:00]   */ .ff1(),          
        /* output [03:00]   */ .ov_msb(),       
        /* output [03:00]   */ .co_msb(),      
        /* output [03:00]   */ .cmsb({cmsb, nc1, nc2, nc3}),        
        /* output [03:00]   */ .so(),           
        /* output [03:00]   */ .f0_bus_stat(),  
        /* output [03:00]   */ .f0_blk_stat(),  
        /* output [03:00]   */ .f1_bus_stat(),  
        /* output [03:00]   */ .f1_blk_stat()   
        );
        end 
    endgenerate
   
  
    /**************************************************************************/
    /* Synchronous procedures                                                 */
    /**************************************************************************/
    
    generate
         /* Time Multiplexing Logic */
        if (TimeMultiplexing == PRS_CFG_TDM_ENABLE)
        begin
            if ((CY_UDB_V0)&&(RunMode == PRS_RUNMODE_CLOCKED))
            begin
                always @(posedge clk or posedge reset_state0 or  posedge reset_state1)
                begin
                    if (reset_state0)
                    begin            
                        state[0] <= 1'b1;
                    end
                    else 
                    if (reset_state1) 
                    begin
                        state[1] <= 1'b1;
                    end
                    else
                    if (enable_final)
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
                always @(posedge clk or posedge reset_so)
                begin
                    
                    if (reset_so)
                    begin
                        so_reg = 1'b1;
                    end
                    else if (save_so)
                    begin
                        so_reg = so;
                    end
                    else so_reg = so_reg;
                end 
                
                /* ci */     
                always @(posedge clk or posedge reset_ci)
                begin
                    if (reset_ci)
                    begin
                        ci_temp = 1'b1;
                    end
                    else if (save)
                    begin
                        ci_temp = ci;
                    end 
                    else ci_temp = ci_temp;
                        
                end 
                
                /* si */     
                always @(posedge clk or posedge reset_si)
                begin
                    if (reset_si)
                    begin
                        sc_temp = 1'b1;
                    end
                    else if (save)
                    begin
                        sc_temp = sc_out[dpPOVal];
                    end
                    else sc_temp = sc_temp;
                end 
            end
            else
            begin
                always @(posedge clk_ctrl)
                begin
                    if (reset_final)
                    begin            
                        state[0] <= ctrl_reset_state[0];
                        state[1] <= ctrl_reset_state[1];
                    end
                    else 
                    if (enable_final)
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
                always @(posedge clk_ctrl)
                begin
                    
                    if (reset_final)
                    begin
                        so_reg = ctrl_reset_so;
                    end
                    else if (save_so & enable_final)
                    begin
                        so_reg = so;
                    end
                    else so_reg = so_reg;
                end 
                
                /* ci */     
                always @(posedge clk_ctrl)
                begin
                    if (reset_final)
                    begin
                        ci_temp = ctrl_reset_ci;
                    end
                    else if (save & enable_final)
                    begin
                        ci_temp = ci;
                    end 
                    else ci_temp = ci_temp;
                        
                end 
                
                /* si */     
                always @(posedge clk_ctrl)
                begin
                    if (reset_final)
                    begin
                        sc_temp = ctrl_reset_si;
                    end
                    else if (save & enable_final)
                    begin
                        sc_temp = sc_out[dpPOVal];
                    end
                    else sc_temp = sc_temp;
                end 
            end
        end
    endgenerate
    
   
    /**************************************************************************/
    /* Combinatinal procedures                                                */
    /**************************************************************************/
    
    /* Datapathes*/
    generate
        if (TimeMultiplexing == PRS_CFG_TDM_ENABLE) 
        begin
            if (Resolution <= PRS_16_BIT)
            begin
                assign so = so_a;  
                assign sc_out = sc_out_a;
            end
            else if (Resolution <= PRS_24_BIT)
            begin
                assign si_b = dcfg ? si : so_a;
                assign so = so_b;
                assign sc_out = sc_out_b;
            end
            else if (Resolution <= PRS_32_BIT)
            begin
                assign si_b = so_a;
                assign so = so_b;
                assign sc_out = sc_out_b;
            end    
            else if (Resolution <= PRS_40_BIT)
            begin  
                assign si_c = so_b;
                assign si_b = dcfg ? si : so_a;
                assign so = so_c;
                assign sc_out = sc_out_c;
            end
            else if (Resolution <= PRS_48_BIT) 
            begin
                assign si_c = so_b;
                assign si_b = so_a;
                assign so = so_c;
                assign sc_out = sc_out_c;
            end
            else if (Resolution <= PRS_56_BIT)
            begin
                assign si_d = so_c;
                assign si_c = so_b;
                assign si_b = dcfg ? si : so_a;
                assign so = so_d;
                assign sc_out = sc_out_d;
            end
            else if (Resolution <= PRS_64_BIT)
            begin
                assign si_d = so_c;
                assign si_c = so_b;
                assign si_b = so_a;
                assign so = so_d;
                assign sc_out = sc_out_d;
            end
			
			assign save = ((state == PRS_STATE_SAVE_LOWER) | (state == PRS_STATE_SAVE_UPPER));
			assign save_so = ~save;
			assign dcfg = ((state == PRS_STATE_CALC_UPPER) | (state == PRS_STATE_SAVE_UPPER));

            assign cs_addr = {reset, state[1:0]};
            assign bitstream = ci_temp;
            assign ci = dcfg ? sc_out[dpPOVal] : ci_temp;
            assign si = dcfg ? so_reg : sc_temp;
        end
		else
        begin
            assign cs_addr = {reset, 1'b0, enable_final};
            assign bitstream = cmsb;
        end
    endgenerate
  
    /* Control Signals */
    assign ctrl_enable       = control[PRS_CTRL_ENABLE];
    assign ctrl_api_clock    = control[PRS_CTRL_RISING_EDGE];
    assign ctrl_reset_common = control[PRS_CTRL_RESET_COMMON];
    assign ctrl_reset_ci     = control[PRS_CTRL_RESET_CI];
    assign ctrl_reset_si     = control[PRS_CTRL_RESET_SI];
    assign ctrl_reset_so     = control[PRS_CTRL_RESET_SO];
    assign ctrl_reset_state  = control[PRS_CTRL_RESET_STATE1 : PRS_CTRL_RESET_STATE0];
    
    /* Status Signals */
    assign status[2:0] = 3'h0; 
    assign status[PRS_CTRL_RESET_CI]     = ci_temp;   
    assign status[PRS_CTRL_RESET_SI]     = sc_temp;
    assign status[PRS_CTRL_RESET_SO]     = so;
    assign status[PRS_CTRL_RESET_STATE0] = state[0];
    assign status[PRS_CTRL_RESET_STATE1] = state[1];
    
    /* Control state Signals*/
    assign enable_final = ctrl_enable & enable;
    assign reset_final  = ctrl_reset_common | reset;
    
    /* Signals is used for recover PRS stete after Sleep in ES1 */
    generate
         /* Time Multiplexing Logic */
        if (TimeMultiplexing == PRS_CFG_TDM_ENABLE)
        begin
            if ((CY_UDB_V0)&&(RunMode == PRS_RUNMODE_CLOCKED))
            begin
                assign reset_ci = reset_final & ctrl_reset_ci;
                assign reset_si = reset_final & ctrl_reset_si;
                assign reset_so = reset_final & ctrl_reset_so;
                assign reset_state0 = reset_final & ctrl_reset_state[0];
                assign reset_state1 = reset_final & ctrl_reset_state[1];   
            end
        end
    endgenerate    

endmodule   /* PRS_v2_10 */
`endif      /* PRS_v2_10_V_ALREADY_INCLUDED */

