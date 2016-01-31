/*******************************************************************************
* File Name: bSegLCD_v2_10.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  The Base SegLCD generates six UDB-based signals for PSoC3 LCD subsystem
*  Implementation done with UDB's.
*
********************************************************************************
*                                IO SIGNALS
********************************************************************************
*   name           IO          Description
*
*   clock         input        component clock
*   drive,        output       drive
*   frame         output       frame
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
* I*O Signals:
********************************************************************************
*    name         direction       Description
*    clock        input           Clock that operates this component
*    lcd_int      input           Wakeup interupt signal from LCD Timer               
*    mode_1       output          One of LCD Driver mode control sinals 
*    mode_2       output          One of LCD Driver mode control sinals 
*    frame        output          Signal that controls waveform generation
*    drive        output          Enable signal for LCD Driver buffers
*    data_clk     output          A clock for LCD data registers
*    clr_int      output          Signal that clears LCD Timer wake up interrupt    
*    lp_ack       output          Rquest signal for entering to a Sleep mode  
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef bSegLCD_v2_10_ALREADY_INCLUDED
`else
`define bSegLCD_v2_10_ALREADY_INCLUDED

module bSegLCD_v2_10 (
	input    clock,       /* Clock that operates this component */
    input    bus_clk,     /* Buss clock required in low power operation */
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
    localparam LCD_EN              = 3'h0; 	                 /* Global enable  */
	localparam LCD_MODE_1          = 3'h1;                   /* Mode 1st bit   */
    localparam LCD_MODE_2          = 3'h2; 	                 /* Mode 2nd bit   */
    localparam LCD_FRAME_DONE      = 3'h3;                   /* DMA Done (FRAME) */
    localparam LCD_FRAME           = 3'h7;                   /* Frame signal */
    
    /***************************************************************************
    *            Combinatorial signals                                         *
    ***************************************************************************/
	
    wire pwm_enable;             /* Changes control store address */
    wire ctl_clk;                /* Clock for control register */
    wire udb_clk;                /* Clock for data path */
    wire n_lp_ack;
    wire n_drive;
    wire tc;
    wire pre_lcd_clk;
	wire pre_drive;
    wire tc_fin;
	wire n_mode_sel;
	wire md_addr;
    reg tmp_wire;
	wire	[7:0]	control;
    
    /* Control Register Outputs */
    wire    ctrl_lcd_enable        = control[LCD_EN];
    wire    ctrl_lcd_frame	       = control[LCD_FRAME];
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
    
    cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h08), .cy_ctrl_mode_0(8'h8f))
    CtrlReg(
        /* input             */     .clock(ctl_clk),
        /* output    [07:00] */     .control(control)
    );
    
    /*  */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) UdbClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(ctrl_lcd_enable),
        /* output */    .clock_out(udb_clk)
    );  
	
    generate
    
    if (PowerMode == 0) begin: NoSleep 
        
        cy_dff lcd_dl_ff
        (
            .d(tc_fin), 
            .q(pre_lcd_clk), 
            .clk(udb_clk)
        );
        
       cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP___OR, `CS_SRCA_A0, `CS_SRCB_A1,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Reset (A0 <= A0 | A1) */
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Decrement A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            8'hFF, 8'h00,	
            8'hFF, 8'hFF,	
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
            `SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
            `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
            `SC_SI_A_ROUTE, 
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
            1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, 
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL 
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
        
		/* Component outputs available in No Sleep */
        assign lp_ack = 1'b0;
        assign clr_int = 1'b0;
        assign mode_1 = n_mode_sel ? 1'b1 : ctrl_lcd_mode1;
        assign mode_2 = n_mode_sel ? 1'b1 : ctrl_lcd_mode2;

    end
    
    else begin: LowPower
       
        wire async_lp_ack;
        wire sync_lcd_int;
        wire lp_ltch_rst;
        wire sync_lp_rst;
        
        /* Pulse generator */
        cy_dff lcd_dl_ff
        (
            .d(lcd_int), 
            .q(sync_lcd_int), 
            .clk(bus_clk)
        );
             
        cy_dff lcd_dl1_ff
        (
            .d(sync_lcd_int), 
            .q(tmp_wire), 
            .clk(udb_clk)
        );
        
        
        assign pre_lcd_clk = sync_lcd_int & ~tmp_wire;
        assign lp_ltch_rst = tc & pwm_enable;
        
        cy_dff lcd_dl2_ff
        (
            .d(lp_ltch_rst), 
            .q(sync_lp_rst), 
            .clk(bus_clk)
        );
        
        
        
        /* Asyncronous latch for lp_ack signal generation */
        cy_srlch lcd_lp_lch
        (
            .s(pre_lcd_clk), 
            .r(sync_lp_rst), 
            .q(n_lp_ack)
        );
        
        assign async_lp_ack = ~n_lp_ack;
         
         cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Reset (A0 <= D0) */
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Decrement A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA,
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, 
            8'hFF, 8'h00,	
            8'hFF, 8'hFF,	
            `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
            `SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
            `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
            `SC_SI_A_ROUTE, 
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
            1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, 
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL 
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
        
        /* Component outputs available in Low Power */
        assign lp_ack = async_lp_ack;
        assign clr_int = pre_lcd_clk;
		assign mode_1 = ctrl_lcd_mode1;
        assign mode_2 = ctrl_lcd_mode2;
        
    end
    
    endgenerate
    
        assign tc_fin = tc & ctrl_lcd_enable;

        LCD_sr_dff_v1_0 lcd_sr_dff1
        (
	        .q(pwm_enable),
            .d(pwm_enable),
	        .clk(ctl_clk),
	        .reset(tc_fin),
            .set(ctrl_lcd_frame_done)
        );

        assign pre_drive = ~n_drive & pwm_enable;
		
	    cy_dff lcd_drive_ff
        (
            .d(pre_drive), 
            .q(drive), 
            .clk(udb_clk)
        );

        /* Common for both modes outputs */    
        assign data_clk = pre_lcd_clk;
        assign frame = ctrl_lcd_frame;

endmodule
`endif /*bSegLCD_v2_10_ALREADY_INCLUDED*/


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
`endif /*LCD_dr_ff_v1_0_ALREADY_INCLUDED*/














