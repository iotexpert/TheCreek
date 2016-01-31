/*******************************************************************************
* File Name: bSegLCD_v1_60.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
*  The Base SegLCD generates six UDB-based signals for PSoC3 LCD subsystem
*  Implementation done with UDB's.
*
*------------------------------------------------------------------------------
*                  Control Register definitions
*------------------------------------------------------------------------------
*
*  Control Register Definition
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  |  Bit  |   7    |   6    |   5    |   4    |   3    |   2    |   1    |   0    |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*  |unused | unused | unused | unused | unused | unused | p_res  |ctrl_res| cnt_en |
*  +=======+--------+--------+--------+--------+--------+--------+--------+--------+
*
*    clk_en   =>       0 = clock disable
*                      1 = clock enable
*                      
*    ctrl_res =>       0 = Normal Operation on the component
*                      1 = Software Reset     
*  
*    p_res   =>        generates a clock pulse for count7 so it can reset
*                      properly  
*
********************************************************************************
* Data Path register definitions
********************************************************************************
* SegLCD: bSegLCDdp
* DESCRIPTION: Implement a 8-Bit down counter
* REGISTER USAGE:
* F0 => not used
* F1 => not used
* D0 => start delay (for dac_disable delay) 
* D1 => value that adjusts the en_hi period
* A0 => (actual counter)
* A1 => 127 (for divider by 128)
*
* SegLCD: bLowPowerdp
* DESCRIPTION: Implement a 8-Bit down counter
* REGISTER USAGE:
* F0 => not used
* F1 => not used
* D0 => Low Drive duration period
* D1 => not used
* A0 => (actual counter)
* A1 => not used
*
********************************************************************************
* I*O Signals:
********************************************************************************
*   name           IO          Description
*
*   clock         input        Clock that operates this component
*   dac_dis       output       Turns off LCD DAC
*   data_clk      output       A clock for LCD data registers 
*   dma_req       output       Starts transition of LCD data
*   drive         output       Enable signal for LCD Driver buffers
*   en_hi         output       Set LCD buffers into Hi Drive mode
*   frame         output       Signal that controls waveform generation
*
********************************************************************************
* Copyright 2008-2011, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef bSegLCD_v1_60_V_ALREADY_INCLUDED
`else
`define bSegLCD_v1_60_V_ALREADY_INCLUDED

module bSegLCD_v1_60 (
	input  wire clock,     /* Clock that operates this component */
	output wire dac_dis,   /* Turns off LCD DAC */
	output wire data_clk,  /* A clock for LCD data registers */
	output wire dma_req,   /* Starts transition of LCD data */
	output wire drive,     /* Enable signal for LCD Driver buffers */
	output wire en_hi,     /* Set LCD buffers into Hi Drive mode */
	output wire frame      /* Signal that controls waveform generation */
);

    /***************************************************************************
    *              Parameters                                                
    ***************************************************************************/
    parameter [4:0] NumCommonLines = 7'd16;
    parameter DriverPowerMode      = 1'b1;
    parameter [7:0] FrameRate      = 8'd30;
    parameter WaveformType         = 1'b1;

    localparam LCD_CNT_EN          = 1'h0; 				/* Clock enable     */
    localparam LCD_RESET		   = 1'h1; 				/* Software reset   */
    localparam LCD_POST_RESET      = 2'h2; 				/* Post reset       */
	localparam mx_ratio            = NumCommonLines-1;  /* Multiplex ratio  */	   
	
    /***************************************************************************
    *            Combinatorial signals                                         *
    ***************************************************************************/
    wire reset = 1'b0;
    wire counter_tc;
    wire zero_detect2;
	wire zero_detect;
	wire zd_tune;
    wire udb_clk;
	wire [7:0] control;         /* Control Register Output    */
    wire internal_drive;
	reg ctrl_store_sm_fin;
	reg zd_tune_fin;
	reg zd_fin;
	wire counter_enable;
	wire intrnl_clock;
	wire fr_type_b;
	wire dac_disable;
	wire ctrl_store_sm;
	wire zd;
	wire change_comm;
	wire count_out,nc1,nc2,nc3,nc4,nc5,nc6,nc7;
	wire internal_clock;
	wire counter_clk;
    wire clk_ff4;
    wire ctrl_cnt_enable = control[LCD_CNT_EN];
    wire ctrl_reset      = control[LCD_RESET];
    wire ctrl_post_reset = control[LCD_POST_RESET];
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) UdbClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(ctrl_cnt_enable),
        /* output */    .clock_out(udb_clk)
    );  
    
	/* Control Register instantiation */
  	cy_psoc3_control
    #(.cy_force_order(1))
    ctrlreg(
        /* output	[07:00]	  */  .control(control)
    );
 
	always@(posedge udb_clk)begin
	    if(ctrl_reset) begin
		    ctrl_store_sm_fin <=1'b0; 
		end 
		else 
        if(zd_tune)begin
		    ctrl_store_sm_fin <= ~ctrl_store_sm_fin;
		end else
        begin
		    ctrl_store_sm_fin <= ctrl_store_sm_fin;
		end
	end
	
	always@(posedge udb_clk) begin
	    zd_tune_fin           <= zd_tune;
		zd_fin                <= zd;
	end	
 
    /* `bSegLCDdp` Data Path is used as frequency divider by 256 to generate the 
    * en_hi signal with ce1 comparator (to adjust en_hi pulse period) and as 
    * divider by 128 for dac_disable signal generation.
    */
	cy_psoc3_dp8 #(.cy_dpconfig_a(
	{
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Reset (A0<=D0) */
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Reset (A0<=D0) */
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Decrement A0 */
		`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGB, /*CS_REG3 Comment:Decrement A0 */
		`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
		`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
		`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
		`CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
		`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
		`CS_CMP_SEL_CFGB, /*CS_REG5 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
		`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
		`CS_CMP_SEL_CFGB, /*CS_REG6 Comment: */
		`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
		`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
		`CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
		`CS_CMP_SEL_CFGB, /*CS_REG7 Comment: */
		  8'hFF, 8'h00,	/*SC_REG4	Comment: */
		  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
		`SC_CMPB_A1_A0, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
		`SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
		`SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
		`SC_SI_A_ROUTE, /*SC_REG6 Comment: */
		`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
		1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
		`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
		`SC_FB_NOCHN, `SC_CMP1_NOCHN,
		`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
		 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
		`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
		`SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
	})) bSegLCDdp(
    /* input */         .clk(udb_clk), 
    /* input [02:00] */ .cs_addr({1'b0,ctrl_cnt_enable,ctrl_store_sm_fin}), 
    /* input */         .route_si(1'b0), 	
    /* input */         .route_ci(1'b0), 	
    /* input */         .f0_load(1'b0),  	
    /* input */         .f1_load(1'b0),  	
    /* input */         .d0_load(1'b0),  	
    /* input */         .d1_load(1'b0),  	
    /* output */        .ce0(), 		
    /* output */        .cl0(), 			
    /* output */        .z0(zero_detect), 	
    /* output */        .ff0(), 			
    /* output */        .ce1(zd_tune), 	
    /* output */        .cl1(), 			
    /* output */        .z1(), 			
    /* output */        .ff1(), 			
    /* output */        .ov_msb(), 		
    /* output */        .co_msb(), 		
    /* output */        .cmsb(), 			
    /* output */        .so(), 			
    /* output */        .f0_bus_stat(), 	
    /* output */        .f0_blk_stat(), 	
    /* output */        .f1_bus_stat(), 	
    /* output */        .f1_blk_stat()  	
    );
 
    assign counter_clk = zd;  
   
        LCD_t_ff_v1_0 t_ff3(
            .q(intrnl_clock),
            .d(1'b1),
            .reset(ctrl_reset),
            .clk(counter_clk)
        );
   
    generate
    
    if (WaveformType==1) begin: TypeB 
       
        /* counter7 is used for generating the frame signal for Type B waveforms */
        cy_psoc3_count7 #(.cy_period(mx_ratio),.cy_route_ld(1),.cy_route_en(1)) 
                DivCounter(
                /*  input		     */  .clock(counter_clk),
                /*  input		     */  .reset(1'b0),
                /*  input		     */  .load(1'b0),
                /*  input		     */  .enable(1'b1),
                /*  output	[06:00]  */  .count({nc6,nc5,nc4,nc3,nc2,nc1,nc7}),
                /*  output		     */  .tc(counter_tc)
        );
        
        /* Generating of Frame (Type B)*/
        LCD_t_ff_v1_0 t_ff2(
            .q(fr_type_b),
            .d(1'b0),
            .reset(ctrl_reset),
            .clk(counter_tc)
        );    
    
    end
    
    endgenerate
    
    /* Generating the DAC disable signal (for Always Active and 
    * Transition Active modes).
    */
    generate
    
    if (DriverPowerMode == 1) begin:LowPower
        
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Reset (A0<=D0) */
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Decrement A0 */
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
            `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
            `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
            `CS_CMP_SEL_CFGB, /*CS_REG5 Comment: */
            `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
            `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
            `CS_CMP_SEL_CFGB, /*CS_REG6 Comment: */
            `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D1,
            `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGB, `CS_SI_SEL_CFGB,
            `CS_CMP_SEL_CFGB, /*CS_REG7 Comment: */
            8'hFF, 8'h00,	/*SC_REG4	Comment: */
            8'hFF, 8'hFF,	/*SC_REG5	Comment: */
            `SC_CMPB_A1_A0, `SC_CMPA_A0_D1, `SC_CI_B_REGIS,
            `SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_ENBL,
            `SC_A_MASK_ENBL, `SC_DEF_SI_0, `SC_SI_B_ROUTE,
            `SC_SI_A_ROUTE, /*SC_REG6 Comment: */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
            1'b0, `SC_FIFO1__A0, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*SC_REG7 Comment: */
            10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
        })) bLowPowerdp(
        /* input */         .clk(udb_clk), /*Clock*/
        /* input [02:00] */ .cs_addr({1'b0,1'b0,change_comm}), 
        /* input */         .route_si(1'b0), 	
        /* input */         .route_ci(1'b0), 	
        /* input */         .f0_load(1'b0),  	
        /* input */         .f1_load(1'b0),  	
        /* input */         .d0_load(1'b0),  	
        /* input */         .d1_load(1'b0),  	
        /* output */        .ce0(), 	        
        /* output */        .cl0(), 			
        /* output */        .z0(zero_detect2), 
        /* output */        .ff0(), 			
        /* output */        .ce1(), 			
        /* output */        .cl1(), 			
        /* output */        .z1(), 			
        /* output */        .ff1(), 			
        /* output */        .ov_msb(), 		
        /* output */        .co_msb(), 		
        /* output */        .cmsb(), 			
        /* output */        .so(), 			
        /* output */        .f0_bus_stat(), 	
        /* output */        .f0_blk_stat(), 	
        /* output */        .f1_bus_stat(), 	
        /* output */        .f1_blk_stat()  	
        );
        
        assign clk_ff4     = zero_detect|zero_detect2;
        
        LCD_t_ff_v1_0 t_ff4(
            .q(change_comm),
            .d(1'b0),
            .reset(ctrl_reset),
            .clk(clk_ff4)
        );
        
        LCD_t_ff_v1_0 t_ff5(
            .q(dac_disable),
            .d(1'b0),
            .reset(zd_tune),
            .clk(zero_detect2)
        );
        
        assign internal_drive = (dac_disable == 1'b1) ? 1'b0 : 1'b1;
        
        end else begin
        
        /* unused wires in this mode */
        assign clk_ff4 = 1'b0;
        
        end
    
    endgenerate 
    
    generate
    
        if (WaveformType==0) begin
            assign internal_clock = (ctrl_reset == 1'b1) ? ctrl_post_reset : intrnl_clock;
        end
        else begin
            assign internal_clock = (ctrl_reset == 1'b1) ? ctrl_post_reset : zd;
        end
    
    endgenerate
    
    LCD_t_ff_v1_0 t_ff7(
        .q(zd),
        .d(1'b0),
        .reset(zero_detect),
        .clk(ctrl_store_sm_fin)
    );
    
    assign en_hi = zd_fin;
    
    generate
    
        if (DriverPowerMode==0) begin
            assign drive   = ctrl_cnt_enable;
            assign dac_dis = 1'b0;
        end
        
        else begin 
            assign drive   = internal_drive;
            assign dac_dis = dac_disable;
        end
        
        if (WaveformType==0) begin
            assign frame    = internal_clock;
            assign data_clk = internal_clock;
            assign dma_req  = internal_clock;
        end
        
        else begin
            assign frame    = fr_type_b;
            assign data_clk = internal_clock;
            assign dma_req  = internal_clock;
        end

    endgenerate

endmodule
`endif /*bSegLCD_v1_60_ALREADY_INCLUDED*/


`ifdef LCD_t_ff_v1_0_V_ALREADY_INCLUDED
`else
`define LCD_t_ff_v1_0_V_ALREADY_INCLUDED

module LCD_t_ff_v1_0 (
	q,
    d,
	clk,
	reset
);
	output reg q;
	input  wire clk;
    input  wire reset;
    input  wire  d;

always@ (posedge reset or posedge clk) begin
    if(reset)begin
        q <= d;
    end
    else begin 
        q <= !q;
    end
end

endmodule
`endif /*LCD_t_ff_v1_0_V_ALREADY_INCLUDED*/ 
