/*******************************************************************************
* File Name: bSegLCD_v3_10.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  The Base SegLCD generates six UDB-based signals for PSoC3 LCD subsystem
*  Implementation done with UDB's.
*
********************************************************************************
*                 Control Register definitions
********************************************************************************
*
*  Control Register Definition
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  |unused | frame  | unused | unused |unused  | fr_done| mode_2 | mode_1 | clk_en |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*
*    clk_en   =>       0 = clock disable
*                      1 = clock enable
*                      
*    ctrl_res =>       0 = Normal Operation on the component
*                      1 = Software Reset     
*
*    mode_1   =>       bit 1 of the mode bitfield 
*
*    mode_2   =>       bit 2 of the mode bitfield
*
*    fr_done  =>       Generates a pulse when this bit is written by DMA.
*                      Informs the hardware that Frame DMA transaction completed. 
*  
*    frame    =>       This bit generates lcd Frame signal wich is set by DMA.
*  
********************************************************************************
*                 Data Path 1 and Data Path 2 register definitions                
********************************************************************************
*
*  INSTANCE NAME: bSegLCDdp 
*  DESCRIPTION: The data path operates as a pulse width modulator(PWM) to 
*               generate an enable signal for LCD Driver with a specific 
*               length. Also it generate a clock signal for data registers and
*               controls LCD Driver mode multiplexers.
*
*  REGISTER USAGE:
*   F0 => na
*   F1 => na
*   D0 => PWM period (Low Power mode), Compare value 1 (No Sleep mode) 
*   D1 => Compare value 2 (No Sleep mode)
*   A0 => (actual counter)
*   A1 => PWM period (No Sleep mode)
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef bSegLCD_v3_10_ALREADY_INCLUDED
`else
`define bSegLCD_v3_10_ALREADY_INCLUDED

module bSegLCD_v3_10 (
    input    clock,       /* Clock that operates this component */
    input    lcd_int,     /* Wakeup interupt signal from LCD Timer */
    output   drive,       /* Enable signal for LCD Driver buffers */
    output   frame,       /* Signal that controls waveform generation */
    output   data_clk,    /* A clock for LCD data registers */
    output   mode_1,      /* One of LCD Driver mode control sinals */
    output   mode_2,      /* One of LCD Driver mode control sinals */
    output   clr_int,     /* Signal that clears LCD Timer wake up interrupt */
    output   lp_ack       /* Rquest signal for entering to a Sleep mode */
);

	/***************************************************************************
    *               Parameters                                                 *
    ***************************************************************************/
    parameter [1:0] PowerMode      = 2'd2;
    
    /* Control register bit locations */
    localparam LCD_CLK_EN          = 3'h0;                  /* Global enable clock enable signal */
    localparam LCD_MODE_1          = 3'h1;                   /* Mode 1st bit */
    localparam LCD_MODE_2          = 3'h2;                  /* Mode 2nd bit */
    localparam LCD_FRAME_DONE      = 3'h3;                   /* DMA Done (FRAME) */
    localparam LCD_FRAME           = 3'h7;                   /* Frame signal */
    
    /***************************************************************************
    *            Combinatorial signals                                         *
    ***************************************************************************/

    wire pwm_enable;             /* Changes control store address */
    wire ctl_clk;                /* Clock for control register */
    wire udb_clk;                /* Clock for data path */
    wire n_drive;                /* Datapath output: negative drive */
    wire tc;                     /* SegLCD PWM's TC output */
    wire pre_lcd_clk;            /* Sync data clock */
    wire lp_wake;                /* Pulse to set lp_ack signal Low */
    wire tc_fin;                 /* Pulse to finish update sequence by PWM disable */
    wire n_mode_sel;             /* Multiplexer control signal for Mode selection */
    wire [7:0] control;          /* Control Register signals */
    
    /* Control Register Outputs */
    wire    ctrl_lcd_clk_enable    = control[LCD_CLK_EN];   
    wire    ctrl_lcd_frame         = control[LCD_FRAME];
    wire    ctrl_lcd_mode1         = control[LCD_MODE_1];
    wire    ctrl_lcd_mode2         = control[LCD_MODE_2];
    wire    ctrl_lcd_frame_done    = control[LCD_FRAME_DONE];
	
    /* The udb_clock_enable primitive component allows to support clock enable 
    * mechanism and specify the intended synchronization behavior for the clock 
    * result. The result clock is applied to the control register and it is always 
    * on.
    */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) CtlClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(ctl_clk)
    ); 
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) UdbClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(ctrl_lcd_clk_enable),
        /* output */    .clock_out(udb_clk)
    );  
	
    cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h08), .cy_ctrl_mode_0(8'h8f))
    CtrlReg(
        /* input             */     .clock(ctl_clk),
        /* output    [07:00] */     .control(control)
    );
    
    generate
    
    if (PowerMode == 0) begin: NoSleep 
        
        /* Generates source signal for data clock output in No Sleep mode */
        cy_dff lcd_dl_ff
        (
            /* input  */ .d(tc_fin), 
            /* output */ .q(pre_lcd_clk), 
            /* input  */ .clk(udb_clk)
        );
        
       cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP___OR, `CS_SRCA_A0, `CS_SRCB_A1,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0: Reset (A0 <= A0 | A1) */
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1: Decrement A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7: Idle*/ 
            8'hFF, 8'h00, /*CFG9: */
            8'hFF, 8'hFF, /*CFG11-10: */
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
            `SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
            `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
            `SC_SI_A_ROUTE, /*CFG13-12: */ 
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14: */ 
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL  /*CFG17-16: */
        })) bSegLCDdp(
        /* input            */ .clk(udb_clk), 
        /* input [02:00]    */ .cs_addr({1'b0, 1'b0, pwm_enable}),
        /* input            */ .route_si(1'b0),
        /* input            */ .route_ci(1'b0),
        /* input            */ .f0_load(1'b0),
        /* input            */ .f1_load(1'b0),
        /* input            */ .d0_load(1'b0),
        /* input            */ .d1_load(1'b0),
        /* output           */ .ce0(),
        /* output           */ .cl0(n_mode_sel),
        /* output           */ .z0(tc),
        /* output           */ .ff0(),
        /* output           */ .ce1(),
        /* output           */ .cl1(n_drive),
        /* output           */ .z1(),
        /* output           */ .ff1(),
        /* output           */ .ov_msb(),
        /* output           */ .co_msb(),
        /* output           */ .cmsb(),
        /* output           */ .so(),
        /* output           */ .f0_bus_stat(),
        /* output           */ .f0_blk_stat(),
        /* output           */ .f1_bus_stat(),
        /* output           */ .f1_blk_stat()
        );
    
		/* Component outputs available in No Sleep mode */
        assign lp_ack = 1'b0;                                /* Not used in NoSleep mode */ 
        assign clr_int = 1'b0;                               /* Not used in NoSleep mode */ 
        assign mode_1 = n_mode_sel ? 1'b1 : ctrl_lcd_mode1;  /* mode 1 should be 1 for Low Drive */
        assign mode_2 = n_mode_sel ? 1'b1 : ctrl_lcd_mode2;  /* mode 2 should be 1 for Low Drive */

    end /* NoSleep */
    
    else begin: LowPower
       
        wire sync_lcd_int;    /* Intermediate signal to generate wakeup pulse */
        wire lp_dl_lcd_int;   /* Intermediate signal to generate wakeup pulse */
        wire lp_pre_rst;      /* Combinatorial signal for  */
        wire lp_rst;          /* lp_rst forces lp_ack output to go high */
        wire lp_in_progress;  /* Low Power mode signals.  */
        
        /* Pulse generator */
        cy_dff lcd_lp_dl_ff
        (
            /* input  */ .d(lcd_int), 
            /* output */ .q(sync_lcd_int), 
            /* input  */ .clk(ctl_clk)
        );
             
        cy_dff lcd_lp_dl_ff1
        (
            /* input  */ .d(sync_lcd_int), 
            /* output */ .q(lp_dl_lcd_int), 
            /* input  */ .clk(ctl_clk)
        );
        
        assign lp_wake = sync_lcd_int & ~lp_dl_lcd_int;
        
        /* Component outputs available in Low Power mode */
        cy_dff lcd_lp_dl_ff3
        (
            /* input  */ .d(lp_wake), 
            /* output */ .q(pre_lcd_clk), 
            /* input  */ .clk(ctl_clk)
        );
        
        assign lp_pre_rst = tc & pwm_enable;
        
        /* lp_rst forces lp_ack output to go high */
        cy_dff lcd_lp_dl_ff2
        (
            /* input  */ .d(lp_pre_rst), 
            /* output */ .q(lp_rst), 
            /* input  */ .clk(udb_clk)
        );
        
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0: Reset (A0 <= D0) */
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1: Decrement A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2: Idle*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6: Idle*/ 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7: Idle*/ 
            8'hFF, 8'h00, /*CFG9: */
            8'hFF, 8'hFF, /*CFG11-10: */
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
            `SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
            `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
            `SC_SI_A_ROUTE, /*CFG13-12: */ 
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14: */ 
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL  /*CFG17-16: */
        })) bSegLCDdp(
        /* input            */ .clk(udb_clk), 
        /* input [02:00]    */ .cs_addr({1'b0, 1'b0, pwm_enable}),
        /* input            */ .route_si(1'b0),
        /* input            */ .route_ci(1'b0),
        /* input            */ .f0_load(1'b0),
        /* input            */ .f1_load(1'b0),
        /* input            */ .d0_load(1'b0),
        /* input            */ .d1_load(1'b0),
        /* output           */ .ce0(),
        /* output           */ .cl0(n_mode_sel),
        /* output           */ .z0(tc),    
        /* output           */ .ff0(),
        /* output           */ .ce1(),
        /* output           */ .cl1(n_drive),
        /* output           */ .z1(),
        /* output           */ .ff1(),
        /* output           */ .ov_msb(),
        /* output           */ .co_msb(),
        /* output           */ .cmsb(),
        /* output           */ .so(),
        /* output           */ .f0_bus_stat(),
        /* output           */ .f0_blk_stat(),
        /* output           */ .f1_bus_stat(),
        /* output           */ .f1_blk_stat()
        );
        
        /* Syncronous SR FF handles components operation i sleep */
        LCD_sr_dff_v1_0 lcd_lp_sr_dff1  
        (
	        /* output */ .q(lp_in_progress),
            /* input  */ .d(lp_in_progress),
	        /* input  */ .clk(ctl_clk),
	        /* input  */ .reset(lp_rst),
            /* input  */ .set(lp_wake)
        );
        
        /* Component outputs available in Low Power */
        assign lp_ack = ~lp_in_progress;  /* Low Power Acknowledge component output */
        assign clr_int = pre_lcd_clk;     /* Clear interrupt component output */
		assign mode_1 = ctrl_lcd_mode1;   /* mode 1 component output*/
        assign mode_2 = ctrl_lcd_mode2;   /* mode 2 component output*/
        
    end /* LowPower */
    
    endgenerate
    
        assign tc_fin = tc & ctrl_lcd_clk_enable;

        LCD_sr_dff_v1_0 lcd_sr_dff1
        (
	        /* output */ .q(pwm_enable),
            /* input  */ .d(pwm_enable),
	        /* input  */ .clk(ctl_clk),
	        /* input  */ .reset(tc_fin),
            /* input  */ .set(ctrl_lcd_frame_done)
        );

        assign drive = ~n_drive & pwm_enable;
		
        /* Common for both modes outputs */    
        assign data_clk = pre_lcd_clk;  /* Data Clock component output*/
        assign frame = ctrl_lcd_frame;  /* Frame component output*/

endmodule
`endif /* bSegLCD_v3_10_ALREADY_INCLUDED */


`ifdef LCD_sr_dff_v1_0_ALREADY_INCLUDED
`else
`define LCD_sr_dff_v1_0_ALREADY_INCLUDED

module LCD_sr_dff_v1_0 (
    q,
    d,
    clk,
    reset,
    set
);
    output reg q;
    input  wire clk;
    input  wire reset;
    input  wire  d;
    input  wire  set;

always@ (posedge clk) begin
   
    if(set)begin
        q <= 1'b1;
    end 
    else if(reset) begin
        q <= 1'b0;
    end
    else begin 
        q <= d;
    end

end

endmodule
`endif /* LCD_Sr_dff_v1_0_ALREADY_INCLUDED */
