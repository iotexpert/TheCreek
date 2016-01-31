/*******************************************************************************
*  FILENAME:  B_Timer_v1_0.v   
*  UM Name:   B_Timer_v1_0
* 
*  DESCRIPTION:
*    The Timer User Module is a 8, 16, 24 or 32-bit capture timer with software
*    capture and event counter.
* 
* ------------------------------------------------------------------------------
*                                 IO SIGNALS
* ------------------------------------------------------------------------------
*    name          IO         Description
* 
*    reset       input        System reset
*    clock       input        Sytem clock
*    capture_in  input        Capture input signal
*    interrupt   output       Interrupt signal output
*    enable      input        Timer enable input signal
*    trigger     input        Enable trigger input signal
*    capture_in  input	      Capture input signal
*    capture_out output       Capture output signal
*    tc          output       Terminal count output
* 
* ------------------------------------------------------------------------------
*                  Data Path register definitions                
* ------------------------------------------------------------------------------
* 
*   INSTANCE NAME:  timerdp                   
* 
*   DESCRIPTION:
*     This data path implements the timer counter and capture functions.
* 
*   REGISTER USAGE:
*    F0 => Timer Capture value 
*    F1 => Unused
*    D0 => Period register
*    D1 => Unused
*    A0 => Counter Value (actual counter)
*    A1 => Unused
* 
* ------------------------------------------------------------------------------
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
*-------------------------------------------------------------------------------
*
*  Todo:
* 
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef B_TIMER_V_v1_0_ALREADY_INCLUDED
`else
`define B_TIMER_V_v1_0_ALREADY_INCLUDED
module B_Timer_v1_0 (
    input	wire	clock, 			/* System clock                           */
	input 	wire	reset, 			/* system reset signal                    */
    input   wire    enable,         /* timer enable input signal              */
	input   wire    trigger,        /* enable trigger input signal            */
	input	wire	capture_in,		/* capture input signal                   */
    output  wire    capture_out,    /* capture output signal                  */
    output  wire    tc,             /* terminal count output                  */
    output  wire    interrupt       /* Interrupt output signal                */
    );

    /* Internal signals */
    wire            per_zero;            /* all datapath's = per_zero */
    wire            capt_fifo_load;  /* Capture the value in A0 into F1 */

    /* Status Signals */
    wire            status_tc;       /* Teminal count */
    reg             capture_last;    /* Registered capture value */

    /* Various signals */
    wire    [3:0]   zeros;
    reg             trig_last;       /* Used to test for edge of the trigger signal */
    reg             trig_rise_detected; /* Identifies that a rising edge has been detected since  */
    reg             trig_fall_detected;
    wire            trigger_polarized;
    wire            trigger_enable;
    wire            timer_enable;
    reg             trigger_reset;
    wire            fifo_load_polarized;
    wire            counter_tc;
    wire            hwEnable;
    wire            fifo_full;
    wire            fifo_nempty;
    
    /* No Connect Wires */
    wire            nc2, nc1, nc0, nc3, nc4, nc5, nc6, nc7, nc8, nc9, nc10, nc11, nc12, nc13, nc14;
    
    /**************************************************************************/
    /* Parameters                                                             */
    /**************************************************************************/
    localparam [7:0]    TIMER_8_BIT    = 8'd8;
    localparam [7:0]    TIMER_16_BIT   = 8'd16;
	localparam [7:0]    TIMER_24_BIT   = 8'd24;
	localparam [7:0]    TIMER_32_BIT   = 8'd32;
	parameter [7:0]	    Resolution = TIMER_8_BIT;
    parameter       CaptureCounterEnabled   = 1'b0;   /* disabled by default (capture every edge) */
    parameter       InterruptOnCapture = 1'b0;   /* Default is to not use the capture counter to generate interrupts*/
    parameter [6:0] Capture_Count = 7'd3;
    
    localparam TIMER_ENMODE_CRONLY    = 2'b00;
    localparam TIMER_ENMODE_HWONLY    = 2'b01;
    localparam TIMER_ENMODE_CR_HW     = 2'b10;
    parameter [1:0] EnableMode          = TIMER_ENMODE_CRONLY; 
    
    localparam      TIMER_RUNMODE_CONTINUOUS    = 2'd0;
    localparam      TIMER_RUNMODE_ONESHOT       = 2'd1;
    localparam      TIMER_RUNMODE_ONESHOTHALT   = 2'd2;    
    parameter [1:0] RunMode = TIMER_RUNMODE_CONTINUOUS;
    
    localparam TIMER_CAP_MODE_NONE  = 3'd0;
    localparam TIMER_CAP_MODE_RISE  = 3'd1;
    localparam TIMER_CAP_MODE_FALL  = 3'd2;
    localparam TIMER_CAP_MODE_BOTH  = 3'd3;
    localparam TIMER_CAP_MODE_SW    = 3'd4;
    parameter [2:0] CaptureMode     = TIMER_CAP_MODE_NONE;
    
    
    localparam TIMER_TRIG_MODE_NONE         = 3'd0;    /* Do Not Trigger */
    localparam TIMER_TRIG_MODE_RISE         = 3'd1;    /* Trigger on Rising Edge Only */
    localparam TIMER_TRIG_MODE_FALL         = 3'd2;    /* Trigger on Falling Edge Only */
    localparam TIMER_TRIG_MODE_BOTH         = 3'd3;    /* Trigger on Either Edge */
    localparam TIMER_TRIG_MODE_SW          = 3'd4;    /* Trigger mode is set in Control register */
    parameter [2:0] TriggerMode             = TIMER_TRIG_MODE_NONE;
    
    
	/***************************************************************************
    * Control Register #1 Bits
    ***************************************************************************/
	localparam TIMER_CTRL_ENABLE	= 8'h7;	 /* Enable the Timer */
	localparam TIMER_CTRL_CMODE1    = 8'h6;	 /* Capture input Polarity Rising Edge */
	localparam TIMER_CTRL_CMODE0    = 8'h5;	 /* Capture input Polarity Falling */
    localparam TIMER_CTRL_TEN       = 8'h4;  /* Trigger Polarity Falling Edge */
    localparam TIMER_CTRL_TMODE1    = 8'h3;  /* Trigger Polarity Falling Edge */
    localparam TIMER_CTRL_TMODE0    = 8'h2;  /* Trigger Polarity Rising Edge */
	localparam TIMER_CTRL_IC1		= 8'h1;	 /* Upper Bit of Interrupt Count */
	localparam TIMER_CTRL_IC0		= 8'h0;	 /* Lower Bit of Interrupt Count */
	wire	[7:0]	control;		         /* Control Register Output */
    
    wire        ctrl_enable;
    wire [1:0]  ctrl_cmode;
    wire        ctrl_ten;
    wire [1:0]  ctrl_tmode;
    wire [1:0]  ctrl_ic;
    
    generate 
    if(EnableMode == TIMER_ENMODE_HWONLY) begin : sEMHW
        assign ctrl_enable         = 1'b1;
    end
    else begin
        assign ctrl_enable      = control[TIMER_CTRL_ENABLE];
    end
    endgenerate

    generate
    if(CaptureMode == TIMER_CAP_MODE_SW) begin : sCMPSW
        assign ctrl_cmode       = {control[TIMER_CTRL_CMODE1],control[TIMER_CTRL_CMODE0]};
    end
    else begin
        assign ctrl_cmode       = CaptureMode;
    end
    endgenerate
    
    generate
    if(TriggerMode == TIMER_TRIG_MODE_SW) begin : sTRGSW
        assign ctrl_tmode       = {control[TIMER_CTRL_TMODE1], control[TIMER_CTRL_TMODE0]};
    end
    else begin
        assign ctrl_tmode       = TriggerMode;
    end
    endgenerate
    
    assign ctrl_ten         = control[TIMER_CTRL_TEN];  //TODO: Can I remove this?
    
    assign ctrl_ic          = {control[TIMER_CTRL_IC1],control[TIMER_CTRL_IC0]};
	
	/***************************************************************************
    * Status RegisterBits
    ***************************************************************************/
	localparam TIMER_STS_TC			    = 8'h0; /* Terminal Count */
	localparam TIMER_STS_CAPT		    = 8'h1; /* Registered Capture Value */
    localparam TIMER_STS_FIFO_FULL      = 8'h2; /* FIFO Full status (real-time) */
    localparam TIMER_STS_FIFO_NEMPTY    = 8'h3; /* FIFO Empty status (real-time) */
    localparam TIMER_STS_FIFO_FULLINT   = 8'h4; /* FIFO Full status for Interrupt (sticky)*/
	wire	[6:0]	status;			        /* Status Register Input */
    
    assign fifo_load_polarized = (ctrl_cmode == TIMER_CAP_MODE_RISE) ? (capture_in & !capture_last) :
                                 (ctrl_cmode == TIMER_CAP_MODE_FALL) ? (!capture_in & capture_last) :
                                 (ctrl_cmode == TIMER_CAP_MODE_BOTH) ? (capture_in ^ capture_last) :
                               /*(ctrl_cmode == TIMER_CAP_MODE_NONE) ? */ 1'b0;
    
    generate
    if (CaptureCounterEnabled == 1) begin : sCapCount
        /***************************************************************************
        * Capture Logic
        ****************************************************************************
        * "capt_fifo_load" is the final signal to F0_load to capture data
        *  Data Capture Options:
        *   * Rising Edge, Falling Edge, or Either Edge
        *   * -or- a count of these edges
        *   * -or- software capture (This is implemented completely in SW no HW req'd
        *
        *   Rising Edge --|\            _____--- If ctr = 1 then remove the resource usage
        *   Falling Edge--| |          |     |
        *   Rise --)))))  | |----------| CTR |------)))
        *           )))))-| |          |_____|       )))
        *   Fall --)))))  | |                         ))) ----- capt_fifo_load
        *                 |/                         )))
        *    CTRL_SEL------|               SW_Capt--))) 
        *
        ***************************************************************************/
        wire cntr_enable;
        wire [6:0] count;
	    reg tmp_fifo_load;
        reg cntr_load;
        assign cntr_enable = ctrl_cmode > 0;
        cy_psoc3_count7 #(.cy_period(Capture_Count-7'd1),.cy_route_ld(1),.cy_route_en(1)) 
        counter(
        /*  input		     */  .clock(fifo_load_polarized),
        /*  input		     */  .reset(reset),
        /*  input		     */  .load(cntr_load),
        /*  input		     */  .enable(cntr_enable),
        /*  output	[06:00]	 */  .count(count),
        /*  output		     */  .tc()
        );

	  always @(posedge clock or posedge reset) begin
		if(reset) begin
			tmp_fifo_load <= 1'b0;
		end
		else begin
		    tmp_fifo_load <= 1'b0;
            cntr_load <= 1'b0;
		    if(count == 7'h1) begin
                cntr_load <= 1'b1;
                if(!fifo_full & !cntr_load) begin
                    tmp_fifo_load <= 1'b1;
                end
                else begin
                    tmp_fifo_load <= 1'b0;
                end
            end
		end
	  end

        assign capt_fifo_load = tmp_fifo_load;// & !fifo_full;
		 
    end
    else begin
        assign capt_fifo_load = fifo_load_polarized & !fifo_full;
    end
    endgenerate
    
    assign status_tc = (ctrl_enable & per_zero) ? 1'b1 : 1'b0;   /* Terminal Count */
    assign tc = status_tc;
    assign hwEnable =     (EnableMode == TIMER_ENMODE_HWONLY) ? enable : 
                          (EnableMode == TIMER_ENMODE_CRONLY) ? ctrl_enable :
                        /*(EnableMode == TIMER_ENMODE_CR_HW) ? */ (enable & ctrl_enable);
    assign capture_out = capt_fifo_load;
    wire capt_fifo_load_int;
    generate
    if (InterruptOnCapture == 1) begin : sIntCapCount
        reg [1:0] int_capt_count;
        reg       capt_int_temp;
        always @(posedge clock or posedge reset) begin
            if(reset) begin
                int_capt_count <= 2'b00;
                capt_int_temp <= 1'b0;
            end
            else if(capt_fifo_load) begin
                if(int_capt_count == ctrl_ic) begin
                    capt_int_temp <= 1'b1;
                    int_capt_count <= 2'b00;
                end
                else begin
                    capt_int_temp <= 1'b0;
                    int_capt_count <= int_capt_count + 2'b01;
                end
            end
            else begin
                capt_int_temp <= 1'b0;
            end
        end
        assign capt_fifo_load_int = capt_int_temp;
    end
    else
        assign capt_fifo_load_int = capt_fifo_load;
    endgenerate
    
    generate
    if(RunMode == TIMER_RUNMODE_CONTINUOUS) begin
        wire runmode_enable;
        assign runmode_enable = trigger_enable & hwEnable;
        assign timer_enable = runmode_enable;
    end
    else if(RunMode == TIMER_RUNMODE_ONESHOT) begin    
        reg     runmode_enable;
        reg     trig_disable;
        always @(posedge clock or posedge reset) begin
            if(reset) begin
                runmode_enable = 1'b0;
                trig_disable = 1'b0;
            end
            else if(!runmode_enable & tc) begin
                runmode_enable = 1'b1;
                trig_disable = 1'b1;
            end
            else if(trigger_enable & !trig_disable & hwEnable)
                runmode_enable = 1'b0;
        end
        assign timer_enable = !runmode_enable;
    end
    else /*RunMode = TIMER_RUNMODE_ONESHOTHALT */ begin
     reg     runmode_enable;
        reg     trig_disable;
        always @(posedge clock or posedge reset) begin
            if(reset) begin
                runmode_enable = 1'b0;
                trig_disable = 1'b0;
            end
            else if(!runmode_enable & (tc | interrupt)) begin
                runmode_enable = 1'b1;
                trig_disable = 1'b1;
            end
            else if(trigger_enable & !trig_disable & hwEnable)
                runmode_enable = 1'b0;
        end
        assign timer_enable = !runmode_enable;
    end    
    endgenerate

    /***************************************************************************
    * Trigger logic
    ****************************************************************************
    * The trigger must be reset on:
    *       1) hardware reset
    *       2) Trigger is disabled
    *       3) Timer is disabled (don't want it to trigger when it's not disabled?)
    * Trigger Polarity defines what the input edge of the trigger signal must be
    *       00) No Trigger, Timer is always enabled
    *       01) Trigger on Rising Edge
    *       10) Trigger on Falling Edge
    *       11) Trigger on Any Edge
    * The trigger must be reset on:
    *       1) hardware reset
    *       2) Trigger is disabled
    *       3) Timer is disabled (don't want it to trigger when it's not disabled?)
    * 
    ***************************************************************************/
    generate 
        if(TriggerMode != TIMER_TRIG_MODE_NONE) begin : sTMEN
            if(TriggerMode == TIMER_TRIG_MODE_SW) begin : sTMSW
                assign trigger_polarized = (ctrl_tmode == TIMER_TRIG_MODE_NONE) ? 1'b1 : 
                                           (ctrl_tmode == TIMER_TRIG_MODE_RISE) ? trig_rise_detected :
                                           (ctrl_tmode == TIMER_TRIG_MODE_FALL) ? trig_fall_detected :
                                         /*(ctrl_tmode == TIMER_TRIG_MODE_BOTH) ? */ (trig_rise_detected | trig_fall_detected);
            end
            else begin
                assign trigger_polarized = (TriggerMode == TIMER_TRIG_MODE_NONE) ? 1'b1 : 
                                           (TriggerMode == TIMER_TRIG_MODE_RISE) ? trig_rise_detected :
                                           (TriggerMode == TIMER_TRIG_MODE_FALL) ? trig_fall_detected :
                                         /*(TriggerMode == TIMER_TRIG_MODE_BOTH) ? */ (trig_rise_detected | trig_fall_detected);
            end
            assign trigger_enable = (ctrl_ten) ? trigger_polarized : 1'b1;
            
            always @(posedge clock or posedge reset) begin
                if(reset)
                    trigger_reset <= 1'b1;
                else if(!ctrl_ten | !ctrl_enable)
                    trigger_reset <= 1'b1;
                else
                    trigger_reset <= 1'b0;
            end
            always @(posedge clock or posedge reset) begin
                if(reset)
                    trig_last <= 1'b0;
                else
                    trig_last <= trigger;
            end
            always @(posedge clock or posedge trigger_reset) begin
                if(trigger_reset) begin
                    trig_rise_detected <= 1'b0;
                    trig_fall_detected <= 1'b0;
                end
                else begin
                    trig_rise_detected <= (!trig_last & trigger) | trig_rise_detected; /* rising edge and the detector isn't already set */
                    trig_fall_detected <= (trig_last & !trigger) | trig_fall_detected; /* falling edge and the detector isn't already set */
                end        
            end
        end
        else begin
            assign trigger_enable = 1'b1;
        end
     endgenerate
    /***************************************************************************
    * End of Trigger logic
    ***************************************************************************/

    always @(posedge clock or posedge reset) begin
       if (reset) begin
          capture_last <= 0;
       end
       else begin
          capture_last <= capture_in;    /* Register the capture signal */
       end
    end
	
    /* Instantiate the control registers */
    cy_psoc3_control
    	#(.cy_force_order(1))	
    ctrlreg(
    /*  output	[07:00]	         */  .control(control)
    );
	
	assign status[6:5] = 2'h0;		/* unused bits of the status register */
	assign status[TIMER_STS_TC] = status_tc;
	assign status[TIMER_STS_CAPT] = capt_fifo_load_int;
    assign status[TIMER_STS_FIFO_FULL] = fifo_full;
    assign status[TIMER_STS_FIFO_NEMPTY] = fifo_nempty;
    assign status[TIMER_STS_FIFO_FULLINT] = fifo_full;

    /* Instantiate the status register and interrupt hook*/
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h13), 
        .cy_int_mask(7'h7F)) 
    stsreg(
    /* input          */  .clock(clock),
    /* input  [06:00] */  .status(status),
    /* output         */  .interrupt(interrupt)
    );
    
    parameter dpconfig0 = 
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG0 Comment:Idle
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG1 Comment:Idle
    	`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1)
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	  8'hFF, 8'h00,	//SC_REG4	Comment:
    	  8'hFF, 8'hFF,	//SC_REG5	Comment:
    	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_DEFSI, //SC_REG6 Comment:
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
    	`SC_CMP0_NOCHN, //SC_REG7 Comment:
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_FX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL //SC_REG8 Comment:
    };
    
    parameter dpconfig1 = 
    {
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG0 Comment:Idle
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG1 Comment:Idle
    	`CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1)
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    	`CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
    	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    	`CS_CMP_SEL_CFGA, //CS_REG3 Comment:Preload Period  ( A0 <= D0 )
    	  8'hFF, 8'h00,	//SC_REG4	Comment:
    	  8'hFF, 8'hFF,	//SC_REG5	Comment:
    	`SC_CMPB_A0_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
    	`SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    	`SC_SI_A_DEFSI, //SC_REG6 Comment:
    	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    	1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
    	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
    	`SC_FB_NOCHN, `SC_CMP1_CHNED,
    	`SC_CMP0_CHNED, //SC_REG7 Comment:
    	 10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_FX,
    	`SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
    	`SC_WRK16CAT_DSBL //SC_REG8 Comment:
    };
    wire [2:0] cs_addr;
    assign cs_addr = {reset,timer_enable,per_zero};
    generate
    if(Resolution <= TIMER_8_BIT) begin : sT8
    cy_psoc3_dp8 #(.cy_dpconfig_a(dpconfig0)) 
    timerdp(
        /*  input                   */  .clk(clock),
        /*  input   [02:00]         */  .cs_addr(cs_addr),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(capt_fifo_load),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output                  */  .ce0(),
        /*  output                  */  .cl0(),
        /*  output                  */  .z0(per_zero), /* Terminal count */
        /*  output                  */  .ff0(),
        /*  output                  */  .ce1(),
        /*  output                  */  .cl1(),
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
    end /* end of if statement for 8 bit timer in generate */
    else if(Resolution <= TIMER_16_BIT) begin : sT16
	assign zeros[3:2] = 2'b11;
    cy_psoc3_dp16 #(.cy_dpconfig_a(dpconfig0), .cy_dpconfig_b(dpconfig1)) 
    timerdp(
        /*  input                   */  .clk(clock),
        /*  input   [02:00]         */  .cs_addr(cs_addr),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(capt_fifo_load),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output  [01:00]         */  .ce0(),
        /*  output  [01:00]         */  .cl0(),
        /*  output  [01:00]         */  .z0({per_zero, nc0}), /* Terminal count */
        /*  output  [01:00]         */  .ff0(),
        /*  output  [01:00]         */  .ce1(),
        /*  output  [01:00]         */  .cl1(),
        /*  output  [01:00]         */  .z1(),
        /*  output  [01:00]         */  .ff1(),
        /*  output  [01:00]         */  .ov_msb(),
        /*  output  [01:00]         */  .co_msb(),
        /*  output  [01:00]         */  .cmsb(),
        /*  output  [01:00]         */  .so(),
        /*  output  [01:00]         */  .f0_bus_stat({fifo_nempty,nc3}),
        /*  output  [01:00]         */  .f0_blk_stat({fifo_full,nc4}),
        /*  output  [01:00]         */  .f1_bus_stat(),
        /*  output  [01:00]         */  .f1_blk_stat()
    );
    end    /* end of else if statement for 16 bit timer in generate */
    else if(Resolution <= TIMER_24_BIT) begin : sT24
    
	assign zeros[3] = 1'b1;
    cy_psoc3_dp24 #(.cy_dpconfig_a(dpconfig0), .cy_dpconfig_b(dpconfig1), 
                    .cy_dpconfig_c(dpconfig1)) 
    timerdp(
        /*  input                   */  .clk(clock),
        /*  input   [02:00]         */  .cs_addr(cs_addr),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(capt_fifo_load),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output  [02:00]         */  .ce0(),
        /*  output  [02:00]         */  .cl0(),
        /*  output  [02:00]         */  .z0({per_zero, nc1, nc0}), /* Terminal count */
        /*  output  [02:00]         */  .ff0(),
        /*  output  [02:00]         */  .ce1(),
        /*  output  [02:00]         */  .cl1(),
        /*  output  [02:00]         */  .z1(),
        /*  output  [02:00]         */  .ff1(),
        /*  output  [02:00]         */  .ov_msb(),
        /*  output  [02:00]         */  .co_msb(),
        /*  output  [02:00]         */  .cmsb(),
        /*  output  [02:00]         */  .so(),
        /*  output  [02:00]         */  .f0_bus_stat({fifo_nempty,nc5,nc6}),
        /*  output  [02:00]         */  .f0_blk_stat({fifo_full,nc7,nc8}),
        /*  output  [02:00]         */  .f1_bus_stat(),
        /*  output  [02:00]         */  .f1_blk_stat()
    );
    end /* end if else statement for 24 bit Timer in generate */
    else begin : sT32
    cy_psoc3_dp32 #(.cy_dpconfig_a(dpconfig0), .cy_dpconfig_b(dpconfig1), 
                    .cy_dpconfig_c(dpconfig1), .cy_dpconfig_d(dpconfig1)) 
    timerdp(
        /*  input                   */  .clk(clock),
        /*  input   [02:00]         */  .cs_addr(cs_addr),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(capt_fifo_load),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output  [03:00]         */  .ce0(),
        /*  output  [03:00]         */  .cl0(),
        /*  output  [03:00]         */  .z0({per_zero, nc2, nc1, nc0}),  /* Terminal count */
        /*  output  [03:00]         */  .ff0(),
        /*  output  [03:00]         */  .ce1(),
        /*  output  [03:00]         */  .cl1(),
        /*  output  [03:00]         */  .z1(),
        /*  output  [03:00]         */  .ff1(),
        /*  output  [03:00]         */  .ov_msb(),
        /*  output  [03:00]         */  .co_msb(),
        /*  output  [03:00]         */  .cmsb(),
        /*  output  [03:00]         */  .so(),
        /*  output  [03:00]         */  .f0_bus_stat({fifo_nempty,nc9,nc10, nc11}),
        /*  output  [03:00]         */  .f0_blk_stat({fifo_full,nc12,nc13, nc14}),
        /*  output  [03:00]         */  .f1_bus_stat(),
        /*  output  [03:00]         */  .f1_blk_stat()
    );
    end /* end else statement for 32 bit Timer in generate */
endgenerate
endmodule
`endif /* B_TIMER_V_v1_0_ALREADY_INCLUDED */

