/*******************************************************************************
 *
 * FILENAME:  B_Counter_v1_50.v
 * UM Name:   B_Counter_v1_0
 *
 * DESCRIPTION:
 *   The Base Counter User Module is a simple 8, 16, 24 or 32-bit counter
 *   Implementation done with UDB's.  It has a setable period between 1 
 *   and 2^Width-1, a compare and terminal count.
 *
 *------------------------------------------------------------------------------
 *                 Data Path register definitions                
 *------------------------------------------------------------------------------
 *
 *  INSTANCE NAME:  counterdp
 *
 *  DESCRIPTION:
 *    Implements the up/down counter 8-Bit U0 only; 16-bit U0 = LSB, U1 = MSB; etc
 *
 *  REGISTER USAGE:
 *   F0 => Counter Capture Register
 *   F1 => na
 *   D0 => Period Register
 *   D1 => Compare Register
 *   A0 => Counter Value (actual counter)
 *   A1 => na
 *
 *------------------------------------------------------------------------------
 *  Data Path States
 *
 *  0 0 0   0   Idle
 *  0 0 1   1   Reload period   ( A0 <= D0 )
 *  0 1 0   2   Decrement A0    ( A0 <= A0 - 1)
 *  0 1 1   3   Reload period   ( A0 <= D0 )
 *  1 0 0   4   Idle
 *  1 0 1   5   Reload period   ( A0 <= D0 )
 *  1 1 0   6   Increment A0    ( A0 <= A0 + 1)
 *  1 1 1   7   Reload period   ( A0 <= D0 )
 *------------------------------------------------------------------------------
 *
 * Todo:
 *
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef B_COUNTER_V_v1_50_ALREADY_INCLUDED
`else
`define B_COUNTER_V_v1_50_ALREADY_INCLUDED
module B_Counter_v1_50 (
    input	wire	reset,           /* system reset signal                   */
    input	wire	clock,           /* System clock                          */
    input   wire    bus_clk,         /* BUS clock                             */
	input	wire	capture,		 /* Capture trigger input                 */
	input	wire	enable,			 /* Enable input                          */
    input   wire    up_ndown,        /* Up Not_Down direction input           */
    input   wire    upcnt,           /* Up count input                        */
    input   wire    dwncnt,          /* Down count input                      */
    output	wire	tc_out,          /* Terminal count output                 */
    output	wire	cmp_out,         /* Compare output                        */
    output	wire	irq_out          /* Interrupt request output signal       */
	);

    /* Internal signals */
    /* Period Compare Outputs */
    wire           per_zero;        /* A0 is equal to Zero                    */
    wire           per_equal;       /* A0 is equal to Period value            */
    wire           per_less;        /* A0 is less than to compare value       */
    wire           per_FF;          /* A0 of all datapaths == 0xFF...         */
    /* Compare output Compare Outputs */
    wire           cmp_equal;       /* A1 is equal to compare value           */
    wire           cmp_less;        /* A1 is less than to compare value       */
    
    /* Fifo Status bits */
    wire           fifo_full;     /* Indication that the capture FIFO is full */
    wire           fifo_nempty;   /* Indication that the capture FIFO is full */
    /* Other Status Bits */
    wire            cmp_out_status; /* Implements Rising Edge detect of cmp_out for status register*/

    /* Other Signals */
    wire           overflow;        /* Counter has overflowed status signal   */
    wire           underflow;       /* Counter has underflowed status signal  */
    wire           dp_clock;        /* Final clock to the Datapath            */
    wire           dp_dir;          /* Final Direction input to datapath      */
    

    /* Unused Datapath Output Signals */
    wire     nc1,nc2,nc3,nc4,nc5,nc6,nc7,nc8,nc9, nc10, nc11, nc12, nc13, nc14, 
             nc15, nc16, nc17, nc18, nc19, nc20, nc21, nc22, nc23, nc24, nc25,
             nc26, nc27, nc28, nc29, nc30, nc31, nc32, nc33, nc34, nc35, nc36,
             nc37, nc38, nc39, nc40, nc41;
             
    /**************************************************************************/
    /* Parameters                                                             */
    /**************************************************************************/
    localparam [7:0]    COUNTER_8_BIT    = 8'd8;
    localparam [7:0]    COUNTER_16_BIT   = 8'd16;
	localparam [7:0]    COUNTER_24_BIT   = 8'd24;
	localparam [7:0]    COUNTER_32_BIT   = 8'd32;    
    parameter [7:0]	Resolution 	= 8'd8;	/* default is 8-bit counter */
    
    localparam COUNTER_ENMODE_CRONLY    = 2'b00;
    localparam COUNTER_ENMODE_HWONLY    = 2'b01;
    localparam COUNTER_ENMODE_CR_HW     = 2'b10;
    parameter [1:0] EnableMode          = COUNTER_ENMODE_CRONLY; 
    
    localparam COUNTER_RUNMODE_CONTINUOUS    = 1'b0;
    localparam COUNTER_RUNMODE_ONESHOT       = 1'b1;
    parameter RunMode = COUNTER_RUNMODE_CONTINUOUS;
    
    localparam COUNTER_CM_Basic                 = 2'd0;
    localparam COUNTER_CM_Clock_And_Direction   = 2'd1;
    localparam COUNTER_CM_Clock_And_Up_Down     = 2'd2;
    parameter [1:0] ClockMode           = COUNTER_CM_Basic;
    
    localparam COUNTER_CMP_MODE_LT      = 3'h1;
    localparam COUNTER_CMP_MODE_LTE     = 3'h2;
    localparam COUNTER_CMP_MODE_EQ      = 3'h0;
    localparam COUNTER_CMP_MODE_GT      = 3'h3;
    localparam COUNTER_CMP_MODE_GTE     = 3'h4;
    localparam COUNTER_CMP_MODE_SW      = 3'h5;
    parameter [2:0] CompareMode         = COUNTER_CMP_MODE_LT;
    
    localparam COUNTER_CAP_MODE_NONE    = 4'h0;
    localparam COUNTER_CAP_MODE_RISE    = 4'h1;
    localparam COUNTER_CAP_MODE_FALL    = 4'h2;
    localparam COUNTER_CAP_MODE_BOTH    = 4'h3;
    localparam COUNTER_CAP_MODE_SW      = 4'h4;
    parameter [2:0] CaptureMode         = COUNTER_CAP_MODE_NONE;
   
    parameter ReloadOnReset      = 1'b0; /* Reload Counter on Reset*/
    parameter ReloadOnOverUnder  = 1'b1; /* Reload Counter on Under/Over Flow   (DEFAULT = TRUE)*/
    parameter ReloadOnCapture    = 1'b0; /* Reload Counter on compare  */
    parameter ReloadOnCompare    = 1'b0; /* Reload Counter on Capture  */
    
    parameter UseInterrupt = 1;
    parameter CompareStatusEdgeSense = 1;
 	
    /**************************************************************************/
    /* Control Register Implementation                                        */
    /**************************************************************************/
	/* Control Register Bits (Bits 6-5 are unused */
	localparam COUNTER_CTRL_CMOD0		= 8'h0;	/* Compare mode               */
    localparam COUNTER_CTRL_CMOD1		= 8'h1;	/* Compare mode               */
    localparam COUNTER_CTRL_CMOD2		= 8'h2;	/* Compare mode               */
    localparam COUNTER_CTRL_CAPMODE0    = 8'h3;	/* Capture mode               */
    localparam COUNTER_CTRL_CAPMODE1    = 8'h4;	/* Capture mode               */
    localparam COUNTER_CTRL_UNUSED      = 8'h6; /* Unused                     */
    localparam COUNTER_CTRL_ENABLE		= 8'h7;	/* Enable Timer               */
	wire	[7:0]	control;		            /* Control Register Output    */
    
    wire         ctrl_enable;
    wire   [2:0] ctrl_cmod;
    wire   [1:0] ctrl_capmode;
    
    generate
    if(CompareMode == COUNTER_CMP_MODE_SW) begin : sCMPSW    
        assign  ctrl_cmod = {control[COUNTER_CTRL_CMOD2],
                             control[COUNTER_CTRL_CMOD1],
                             control[COUNTER_CTRL_CMOD0]};
    end
    else begin
        assign ctrl_cmod = CompareMode;
    end
    endgenerate

    generate
    if(CaptureMode == COUNTER_CAP_MODE_SW) begin : sCAPSW
        assign  ctrl_capmode =  {control[COUNTER_CTRL_CAPMODE1],
                                 control[COUNTER_CTRL_CAPMODE0]};
    end
    else begin
        assign ctrl_capmode = CaptureMode;
    end
    endgenerate 
    
    generate
    if((EnableMode != COUNTER_ENMODE_HWONLY) || (CompareMode == COUNTER_CMP_MODE_SW) || (CaptureMode == COUNTER_CAP_MODE_SW)) begin : sCTRLReg
        assign ctrl_enable    = control[COUNTER_CTRL_ENABLE];
        /* Instantiate the control register */
        cy_psoc3_control
        	#(.cy_force_order(1))
        ctrlreg(
            /*  output	[07:00]	         */  .control(control)
        );
    end
    else begin
        assign ctrl_enable = 1'b1;
        assign control = 8'd0;
    end
    endgenerate
    
    /**************************************************************************/
    /*  Capture input implementation                                          */
    /**************************************************************************/
    wire     hwCapture;
    reg      capt_either_edge;
    
    reg prevCapture;
    always @(posedge bus_clk) begin
        prevCapture <= capture;
    end
    wire capt_rising;
    wire capt_falling;
    assign capt_rising = !prevCapture & capture;
    assign capt_falling = prevCapture & !capture;
    
    always @(posedge bus_clk) begin
        capt_either_edge <= (capt_rising | capt_falling);   
    end
    
    assign hwCapture =  (ctrl_capmode == COUNTER_CAP_MODE_NONE) ? 1'b0 :
                        (ctrl_capmode == COUNTER_CAP_MODE_RISE) ? capt_rising :
                        (ctrl_capmode == COUNTER_CAP_MODE_FALL) ? capt_falling :
                        /*(CaptureMode == COUNTER_CAP_MODE_BOTH) ?*/ capt_either_edge;
                        
    
    /**************************************************************************/
    /* Reload, Reset and Enable Implementations                               */
    /**************************************************************************/
    wire    final_reset = reset;
    
    wire    reload;
    assign  reload = (ReloadOnReset & final_reset) | 
                     (ReloadOnOverUnder & (overflow | underflow)) |
                     (ReloadOnCapture & hwCapture) |
                     (ReloadOnCompare & cmp_out);
    
    /* Enable handling */
       
    wire final_enable;
    assign final_enable = (EnableMode == COUNTER_ENMODE_CRONLY) ? ctrl_enable :
                          (EnableMode == COUNTER_ENMODE_HWONLY) ? enable :
                        /*(EnableMode == COUNTER_ENMODE_CR_HW) ?*/(ctrl_enable & enable);
                        
   
    /**************************************************************************/
    /* RunMode Implementations                               */
    /**************************************************************************/	
    wire counter_enable;
    
    if(RunMode == COUNTER_RUNMODE_CONTINUOUS) begin
        assign counter_enable = final_enable;
    end
    else if(RunMode == COUNTER_RUNMODE_ONESHOT) begin   
        reg runmode_enable;
        reg counter_disable;
        always @ (posedge clock or posedge reset) begin
            if (reset) begin
                runmode_enable = 1'b0;
                counter_disable = 1'b0;
            end
            else if(runmode_enable & tc_out) begin
                runmode_enable = 1'b0;
                counter_disable = 1'b1;
            end
            else if(!counter_disable & final_enable) begin
                runmode_enable = 1'b1;
            end
        end
        
        assign counter_enable = runmode_enable;
    end
    
    /**************************************************************************/
    /* Status Register Implementation                                         */
    /**************************************************************************/
	localparam COUNTER_STS_CMP  		= 8'h0; /* Compare output             */
	localparam COUNTER_STS_A0ZERO		= 8'h1; /* A0 Zero ouput              */
    localparam COUNTER_STS_OVERFLOW     = 8'h2; /* Overflow status            */
    localparam COUNTER_STS_UNDERFLOW    = 8'h3; /* Underflow                  */
    localparam COUNTER_STS_CAPTURE      = 8'h4; /* Capture Status             */
    localparam COUNTER_STS_FIFO_FULL    = 8'h5; /* FIFO Full Status           */
    localparam COUNTER_STS_FIFO_NEMPTY  = 8'h6; /* FIFO Not Empty Status      */
	
    /***************************************************************************
    *          Device Family and Silicon Revision definitions
    ***************************************************************************/
    
    /* PSoC3 ES2 or earlier */
    localparam PSOC3_ES3  =  ((`CYDEV_CHIP_FAMILY_USED == `CYDEV_CHIP_FAMILY_PSOC3) && 
                              (`CYDEV_CHIP_REVISION_USED > `CYDEV_CHIP_REVISION_3A_ES2));
                                    
    /* PSoC5 ES1 or earlier */                        
    localparam PSOC5_ES2  =  ((`CYDEV_CHIP_FAMILY_USED == `CYDEV_CHIP_FAMILY_PSOC5) &&
                              (`CYDEV_CHIP_REVISION_USED > `CYDEV_CHIP_REVISION_5A_ES1));
  	

    generate
    if(UseInterrupt) begin : sSTSReg
        wire	[6:0]	status;			                    /* Status Register Input      */
        assign status[COUNTER_STS_CMP] = cmp_out_status;    /* Edge Sensitive Compare implementation for Compare output*/
        assign status[COUNTER_STS_A0ZERO] = per_zero;       /* Already basically edge sense should be 1 clock cycle in most instances */
        assign status[COUNTER_STS_OVERFLOW] = overflow;     /* Already basically edge sense should be 1 clock cycle in most instances */
        assign status[COUNTER_STS_UNDERFLOW] = underflow;   /* Already basically edge sense should be 1 clock cycle in most instances */
        assign status[COUNTER_STS_CAPTURE] = hwCapture;     /* Already implements edge sense to define hwCapture*/
        
        assign status[COUNTER_STS_FIFO_FULL] = fifo_full;
        assign status[COUNTER_STS_FIFO_NEMPTY] = fifo_nempty;
        /* Instantiate the status register and interrupt hook*/
		if(PSOC3_ES3 || PSOC5_ES2)
        begin :rstSts
            cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h1F), 
                .cy_int_mask(7'h7F)) 
            stsreg(
            /* input          */  .clock(bus_clk),
            /* input  [06:00] */  .status(status),
            /* output         */  .interrupt(irq_out),
			/* input          */  .reset(reset)
            );
		end	
	    else
        begin :nrstSts	
            cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h1F), 
            .cy_int_mask(7'h7F)) 
            stsreg(
            /* input          */  .clock(bus_clk),
            /* input  [06:00] */  .status(status),
            /* output         */  .interrupt(irq_out)
	        );
		end    
    end
    endgenerate
    
    /**************************************************************************/
    /* Outputs                                                                */
    /**************************************************************************/
    /* Define overflow and underflow */
    /* Overflow if counting up and we reload or roll over to zero */
    /* Underflow if counting down and we reload or roll over to FF */

    assign overflow = ((ClockMode == COUNTER_CM_Clock_And_Up_Down) || (ClockMode == COUNTER_CM_Clock_And_Direction)) ? per_FF : (dp_dir & per_equal);
    assign underflow = ((ClockMode == COUNTER_CM_Clock_And_Up_Down) || (ClockMode == COUNTER_CM_Clock_And_Direction)) ? per_zero : (!dp_dir & per_zero);
    
    /* Compare output         */
    generate
    if(CompareMode == COUNTER_CMP_MODE_SW) begin : sCMPSW_OUTS
        assign cmp_out = (ctrl_cmod == COUNTER_CMP_MODE_LT)  ? cmp_less :
                         (ctrl_cmod == COUNTER_CMP_MODE_LTE) ? (cmp_less | cmp_equal) :
                         (ctrl_cmod == COUNTER_CMP_MODE_EQ)  ? cmp_equal :
                         (ctrl_cmod == COUNTER_CMP_MODE_GT)  ? (!cmp_less & !cmp_equal):
                       /*(ctrl_cmod == COUNTER_CMP_MODE_GTE)?*/  !cmp_less;
    end
    else begin
        assign cmp_out = (CompareMode == COUNTER_CMP_MODE_LT)  ? cmp_less :
                         (CompareMode == COUNTER_CMP_MODE_LTE) ? (cmp_less | cmp_equal) :
                         (CompareMode == COUNTER_CMP_MODE_EQ)  ? cmp_equal :
                         (CompareMode == COUNTER_CMP_MODE_GT)  ? (!cmp_less & !cmp_equal):
                       /*(CompareMode == COUNTER_CMP_MODE_GTE)?*/  !cmp_less;
    end
    endgenerate                 
     
    /* Terminal Count Output */
    generate
    if(ClockMode == COUNTER_CM_Clock_And_Up_Down) begin : sTCOUT
        assign tc_out = per_zero;
    end
    else begin
        assign tc_out = overflow | underflow;
    end
    endgenerate
    
    generate
    if(CompareStatusEdgeSense == 1) begin : sCMPEdgeSense
        reg prevCompare;
        always @(posedge clock) begin
            prevCompare <= cmp_out;
        end
        assign cmp_out_status = !prevCompare & cmp_out;
    end
    else begin
        assign cmp_out_status = cmp_out;
    end
    endgenerate
    
    /**************************************************************************/
    /* Up & Down Counter Implementation                                       */
    /**************************************************************************/
    wire dir_valid;        
    reg      upcnt_stored;
    reg      dwncnt_stored;
    reg      count_up;
    reg      count_down;
    generate 
    if(ClockMode == COUNTER_CM_Clock_And_Up_Down) begin : UpDwn
        
        always @(posedge clock) begin
            if(upcnt) begin
                if(!upcnt_stored) begin
                    upcnt_stored <= 1'b1;
                    count_up <= 1'b1;
                end
                else begin
                    count_up <= 1'b0;
                end
            end
            else begin
                upcnt_stored <= 1'b0;
                count_up <= 1'b0;
            end
        end
        
        always @(posedge clock) begin
            if(dwncnt) begin
                if(!dwncnt_stored) begin
                    dwncnt_stored <= 1'b1;
                    count_down <= 1'b1;
                end
                else begin
                    count_down <= 1'b0;
                end
            end
            else begin
                dwncnt_stored <= 1'b0;
                count_down <= 1'b0;
            end
        end
        
        assign dir_valid = count_up ^ count_down;
        /* Direction is assigned based on whether we've already counted up for a rising edge */
        /* Or already counted down for a down edge */
        assign dp_dir = (upcnt | upcnt_stored) & (!upcnt_stored | !dwncnt | dwncnt_stored);
        assign dp_clock = clock & dir_valid;
    end
    else begin
        assign dp_clock = clock;
        assign dp_dir = up_ndown;
    end
    endgenerate
    
    /**************************************************************************/
    /* Instantiate the data path elements                                     */
    /**************************************************************************/
    wire [2:0] cs_addr = {dp_dir,counter_enable,reload};
    
    parameter dpconfig0stdrd =    
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Idle */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Preload Period (A0 <= D0) */
    	`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1 ) */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Preload Period (A0 <= D0) */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Idle */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Preload Period (A0 <= ZERO)*/
    	`CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Inc A0  ( A0 <= A0 + 1 ) */
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Preload Period (A0 <= ZERO)*/
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_DEFSI, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h00, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment:G8	Comment */
    };
    
    parameter dpconfig1stdrd = 
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG0 Comment:Idle
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG1 Comment:Preload Period (A0 <= D0)
    	`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period (A0 <= D0)
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG4 Comment:Idle
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG5 Comment:Preload Period (A0 <= ZERO)
    	`CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG6 Comment:Inc A0  ( A0 <= A0 + 1 )
    	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG7 Comment:Preload Period (A0 <= ZERO)                     
    	  8'hFF, 8'h00,	//SC_REG4	Comment:                              
    	  8'hFF, 8'hFF,	//SC_REG5	Comment:                              
    	`SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN,
    	`SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_DEFSI, //SC_REG6 Comment:                              
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, //SC_REG7 Comment:                              
    	 10'h00, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL 
    };
    
    parameter dpconfig0dir =    
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:Idle */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:Preload Period (A0 <= D0) */
    	`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1 ) */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:Preload Period (A0 <= D0) */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:Idle */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:Preload Period (A0 <= D0) */
    	`CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:Inc A0  ( A0 <= A0 + 1 ) */
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:Preload Period (A0 <= D0) */
    	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
    	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
    	`SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_DEFSI, /*SC_REG6 Comment: */
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
    	 10'h00, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL /*SC_REG8 Comment:G8	Comment */
    };
    
    parameter dpconfig1dir = 
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG0 Comment:Idle
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG1 Comment:Preload Period (A0 <= D0)
    	`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period (A0 <= D0)
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG4 Comment:Idle
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG5 Comment:Preload Period (A0 <= D0)
    	`CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG6 Comment:Inc A0  ( A0 <= A0 + 1 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG7 Comment:Preload Period (A0 <= D0)                    
    	  8'hFF, 8'h00,	//SC_REG4	Comment:                              
    	  8'hFF, 8'hFF,	//SC_REG5	Comment:                              
    	`SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN,
    	`SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_DEFSI, //SC_REG6 Comment:                              
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
    	`SC_FB_CHNED, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, //SC_REG7 Comment:                              
    	 10'h00, `SC_FIFO_CLK_BUS,`SC_FIFO_CAP_FX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL 
    };


    parameter dpconfig0 = ((ClockMode == COUNTER_CM_Clock_And_Up_Down) || (ClockMode == COUNTER_CM_Clock_And_Direction)) ? dpconfig0dir : dpconfig0stdrd;
    parameter dpconfig1 = ((ClockMode == COUNTER_CM_Clock_And_Up_Down) || (ClockMode == COUNTER_CM_Clock_And_Direction)) ? dpconfig1dir : dpconfig1stdrd;
	
    generate 
	if(Resolution == COUNTER_8_BIT) begin : sC8
	cy_psoc3_dp8 #(.cy_dpconfig_a(dpconfig0)) 
	counterdp(
    /*  input                   */  .clk(dp_clock),
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),
    /*  input                   */  .route_ci(1'b0),
    /*  input                   */  .f0_load(hwCapture),
    /*  input                   */  .f1_load(1'b0),
    /*  input                   */  .d0_load(1'b0),
    /*  input                   */  .d1_load(1'b0),
    /*  output                  */  .ce0(per_equal),
    /*  output                  */  .cl0(per_less),
    /*  output                  */  .z0(per_zero),              /* Terminal Count (A0 == 0)  */
    /*  output                  */  .ff0(per_FF),
    /*  output                  */  .ce1(cmp_equal),            /* Compare output ( A0 == D1 ) */
    /*  output                  */  .cl1(cmp_less),             /* Compare output ( A0 < D1 )  */
    /*  output                  */  .z1(),
    /*  output                  */  .ff1(),
    /*  output                  */  .ov_msb(),
    /*  output                  */  .co_msb(),
    /*  output                  */  .cmsb(),
    /*  output                  */  .so(),
    /*  output                  */  .f0_bus_stat(fifo_nempty), 
    /*  output                  */  .f0_blk_stat(fifo_full),
    /*  output                  */  .f1_bus_stat(),
    /*  output                  */  .f1_blk_stat()
    );
	end //end of if statement for 8 bit section of generate
	else if(Resolution <= COUNTER_16_BIT) begin : sC16
    cy_psoc3_dp16 #(.cy_dpconfig_a(dpconfig0), .cy_dpconfig_b(dpconfig1)) 
    counterdp(
    /*  input                   */  .clk(dp_clock),
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),
    /*  input                   */  .route_ci(1'b0),
    /*  input                   */  .f0_load(hwCapture),
    /*  input                   */  .f1_load(1'b0),
    /*  input                   */  .d0_load(1'b0),
    /*  input                   */  .d1_load(1'b0),
    /*  output  [01:00]         */  .ce0({per_equal, nc16}),
    /*  output  [01:00]         */  .cl0({per_less, nc17}),
    /*  output  [01:00]         */  .z0({per_zero,nc1}),       /* Terminal count ( A0 == 0 ) */
    /*  output  [01:00]         */  .ff0({per_FF, nc10}),
    /*  output  [01:00]         */  .ce1({cmp_equal,nc2}),     /* Compare output ( A0 == D1 ) */
    /*  output  [01:00]         */  .cl1({cmp_less,nc3}),      /* Compare output ( A0 < D1 ) */
    /*  output  [01:00]         */  .z1(),
    /*  output  [01:00]         */  .ff1(),
    /*  output  [01:00]         */  .ov_msb(),
    /*  output  [01:00]         */  .co_msb(),
    /*  output  [01:00]         */  .cmsb(),
    /*  output  [01:00]         */  .so(),
    /*  output  [01:00]         */  .f0_bus_stat({fifo_nempty,nc30}),
    /*  output  [01:00]         */  .f0_blk_stat({fifo_full,nc31}),
    /*  output  [01:00]         */  .f1_bus_stat(),
    /*  output  [01:00]         */  .f1_blk_stat()
    );
	end /*end of else statement of 16 bit section of generate*/
    else if(Resolution <= COUNTER_24_BIT) begin : sC24
    cy_psoc3_dp24 #(.cy_dpconfig_a(dpconfig0), .cy_dpconfig_b(dpconfig1), .cy_dpconfig_c(dpconfig1)) 
    counterdp(
    /*  input                   */  .clk(dp_clock),
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),
    /*  input                   */  .route_ci(1'b0),
    /*  input                   */  .f0_load(hwCapture),
    /*  input                   */  .f1_load(1'b0),
    /*  input                   */  .d0_load(1'b0),
    /*  input                   */  .d1_load(1'b0),
    /*  output  [02:00]         */  .ce0({per_equal,nc18,nc19}),
    /*  output  [02:00]         */  .cl0({per_less,nc20,nc21}),
    /*  output  [02:00]         */  .z0({per_zero,nc1,nc2}),       /* Terminal count ( A0 == 0 ) */
    /*  output  [02:00]         */  .ff0({per_FF, nc11, nc12}),
    /*  output  [02:00]         */  .ce1({cmp_equal,nc3,nc4}),     /* Compare output ( A0 == D1 ) */
    /*  output  [02:00]         */  .cl1({cmp_less,nc5,nc6}),      /* Compare output ( A0 < D1 ) */
    /*  output  [02:00]         */  .z1(),
    /*  output  [02:00]         */  .ff1(),
    /*  output  [02:00]         */  .ov_msb(),
    /*  output  [02:00]         */  .co_msb(),
    /*  output  [02:00]         */  .cmsb(),
    /*  output  [02:00]         */  .so(),
    /*  output  [02:00]         */  .f0_bus_stat({fifo_nempty,nc32,nc33}),
    /*  output  [02:00]         */  .f0_blk_stat({fifo_full,nc34,nc35}),
    /*  output  [02:00]         */  .f1_bus_stat(),
    /*  output  [02:00]         */  .f1_blk_stat()
    );
	end /*end of else statement of 24 bit section of generate*/
    else if(Resolution <= COUNTER_32_BIT) begin : sC32
    cy_psoc3_dp32 #(.cy_dpconfig_a(dpconfig0), .cy_dpconfig_b(dpconfig1), .cy_dpconfig_c(dpconfig1), .cy_dpconfig_d(dpconfig1)) 
    counterdp(
    /*  input                   */  .clk(dp_clock),
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),
    /*  input                   */  .route_ci(1'b0),
    /*  input                   */  .f0_load(hwCapture),
    /*  input                   */  .f1_load(1'b0),
    /*  input                   */  .d0_load(1'b0),
    /*  input                   */  .d1_load(1'b0),
    /*  output  [03:00]         */  .ce0({per_equal,nc24,nc25,nc26}),
    /*  output  [03:00]         */  .cl0({per_less,nc27,nc28,nc29}),
    /*  output  [03:00]         */  .z0({per_zero,nc1,nc2,nc7}),       /* Terminal count ( A0 == 0 ) */
    /*  output  [03:00]         */  .ff0({per_FF, nc13, nc14, nc15}),
    /*  output  [03:00]         */  .ce1({cmp_equal,nc3,nc4,nc8}),     /* Compare output ( A0 == D1 ) */
    /*  output  [03:00]         */  .cl1({cmp_less,nc5,nc6,nc9}),      /* Compare output ( A0 < D1 ) */
    /*  output  [03:00]         */  .z1(),
    /*  output  [03:00]         */  .ff1(),
    /*  output  [03:00]         */  .ov_msb(),
    /*  output  [03:00]         */  .co_msb(),
    /*  output  [03:00]         */  .cmsb(),
    /*  output  [03:00]         */  .so(),
    /*  output  [03:00]         */  .f0_bus_stat({fifo_nempty,nc36,nc37,nc38}),
    /*  output  [03:00]         */  .f0_blk_stat({fifo_full,nc39,nc40,nc41}),
    /*  output  [03:00]         */  .f1_bus_stat(),
    /*  output  [03:00]         */  .f1_blk_stat()
    );
	end /*end of else statement of 32 bit section of generate*/
	endgenerate

endmodule

`endif /*B_COUNTER_V_v1_10_ALREADY_INCLUDED*/
