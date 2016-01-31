/*******************************************************************************
 *
 * FILENAME:         B_FanController_v2_0.v
 * Component Name:   B_FanController_v2_0
 *
 * DESCRIPTION:
 *  This file include implementation of base Fan Controller component. The major
 *  part of the implementation is consists of Open Loop, Closed Loop PWMs that
 *  declared as primitives. Implementation also utilizes number of control and 
 *  status registers.
 *
 *  The OpenLoopPWM component is a resource minimized standard 8-bit or 10-bit
 *  dual output PWM.
 *   
 *  The ClosedLoopPWM component is designed to work with the FanTach component.
 *  Together, these 2 blocks implement closed loop hardware fan control. The
 *  ClosedLoopPWM block generates a single PWM output with 8 or 10 bit 
 *  resolution under customizer control, similar to a standard PWM.
 * 
 *  The B_Alert Status block instantiates a statusi register to generate an
 *  alert output from fan stall or speed regulation errors.
 *
 *******************************************************************************
 *                 Datapath Register Definitions
 *******************************************************************************
 *
 *  INSTANCE NAME:  ClosedLoopFan8  (8-bit resolution) or 
 *                  ClosedLoopFan10 (10-bit resolution)
 *
 *  DESCRIPTION:
 *    Custom PWM module with single output. Programmable max duty cycle.  
 *
 *  REGISTER USAGE:
 *    F0 => not used
 *    F1 => not used
 *    D0 => Maximum PWM duty cycle
 *    D1 => PWM period
 *    A0 => PWM duty cycle
 *    A1 => PWM down counter (working register)
 *
 *    Note: D0 and D1 should both be initialized to the desired PWM period
 * 
 *  DATA PATH STATES:
 *    0 0 0   0   PWM mode (A1->A1-1)
 *    0 0 1   1   Increment Duty Cycle (A0->A0+1)
 *    0 1 0   2   Decrement Duty Cycle (A0->A0-1)
 *    0 1 1   3   Hold Duty Cycle (A0->A0)
 *    1 0 0   4   Pass
 *    1 0 1   5   Pass
 *    1 1 0   6   Pass
 *    1 1 1   7   Pass
 *
 *
 *  INSTANCE NAME:  OpenLoopFan8  (8-bit resolution) or 
 *                  OpenLoopFan10 (10-bit resolution)
 *
 *  DESCRIPTION:
 *    This data path implements the counter, terminal count and both 
 *    compare registers.
 *
 *  REGISTER USAGE:
 *    F0 => FIFO buffer into D0 for duty cycle of PWM output 1
 *    F1 => FIFO buffer into D1 for duty cycle of PWM output 2
 *    D0 => Duty cycle for PWM output 1
 *    D1 => Duty cycle for PWM pwm output 2 
 *    A0 => PWM down counter counter. Reloaded by adding A0 and A1 together.
 *    A1 => PWM period
 *
 *  DATA PATH STATES:
 *    0 0 0   0   PWM mode (A0<-A0-1) 
 *    0 0 1   1   PWM reload (A0<-A0+A1)
 *    0 1 0   2   Pass
 *    0 1 1   3   Pass
 *    1 0 0   4   Pass
 *    1 0 1   5   Pass
 *    1 1 0   6   Pass
 *    1 1 1   7   Pass
 *
 ********************************************************************************
 *                 I*O Signals:
 ********************************************************************************
 *    Name              Direction       Description
 *    clock             input           Component input clock
 *    addr[3:0]         input           Address bus to the ClosedLoopPWMs
 *    speed_dn          input           Input for a speed down signal from the 
 *                                      FanTach
 *    speed_up          input           Input for a speed up signal from the 
 *                                      FanTach
 *    stall             input           Fan stall alert signals from FanTach for
                                        each enabled fan
 *    pwm               output          PWM output with variable duty cycle to 
 *                                      control the speed of the fans
 *    en                output          B_FanController component enable
 *    alert             output          Active high output pin asserted when fan
 *                                      faults are detected
 *    eoc               input           FanTach component end-of-cycle sync flag
 *    ovrd              output          Output of Override bit from global 
 *                                      control register
 *    alt_mask          output          Mask to enable/disable stall alert 
 *                                      monitoring for each fan
 *
 ********************************************************************************
 * Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
 * You may use this file only in accordance with the license, terms, conditions,
 * disclaimers, and limitations in the end user license agreement accompanying
 * the software package with which this file was provided.
 *********************************************************************************/

`include "cypress.v"

`ifdef B_FanController_v2_0_ALREADY_INCLUDED
`else
`define B_FanController_v2_0_ALREADY_INCLUDED

module B_FanController_v2_0 (
    input  wire         clock,            /* Component input clock */
    input  wire         bus_clock,        /* Bus Clock */
    input  wire         nrq,              /* DMA transfer completion signal */
    input  wire [15:0]  stall,            /* Fan stall alert signals from FanTach for each enabled fan */
    input  wire         spd_up,           /* Input for a speed up signal from the FanTach */
    input  wire         spd_dn,           /* Input for a speed down signal from the FanTach */
    input  wire [3:0]   addr,             /* Address bus to the ClosedLoopPWMs */
    output wire         ovrd,             /* Output of Override bit from global control register */
    output wire         eoc,              /* FanTach component end-of-cycle sync flag */
    output wire         en,               /* B_FanController component enable */
    output wire         alert,            /* Active high output pin asserted when fan faults are detected */
    output wire [15:0]  pwm,              /* PWM output with variable duty cycle to control the speed of the fans */
    output wire [15:0]  alt_mask          /* Mask to enable/disable stall alert monitoring for each fan */
);
     
    parameter Resolution = 4'h0;          /* Resolution: 8-bit or 10-bit */
    parameter Mode       = 1'b0;          /* Mode: Closed or Open loop */
    parameter ErrorCount = 4'h0;          /* Speed regulation error count before generating alert */
    parameter NumOfFans  = 4'h1;          /* Number of FANs in the design */
    
    /* Control register bits */
    localparam ALERT_PIN_EN    = 3'h0;     /* Alert Enable bit index in control register */
    localparam STALL_ALERT_EN  = 3'h1;     /* Stall Alert Enable bit index in control register */ 
    localparam SPEED_ALERT_EN  = 3'h2;     /* Speed Alert Enable bit index in control register */ 
    localparam STALL_ALERT_CLR = 3'h3;     /* Stall Alert Clear bit index in control register */ 
    localparam SPEED_ALERT_CLR = 3'h4;     /* Speed Alert Clear bit index in control register */ 
    localparam ENABLE          = 3'h5;     /* Global enable bit index in control register */ 
    localparam OVERRIDE        = 3'h6;     /* Override hardware control bit index in control register */     
    
    /* PWM indexes */
    localparam PWM0            = 4'h0;     /* PWM #0 index */
    localparam PWM1            = 4'h1;     /* PWM #1 index */
    localparam PWM2            = 4'h2;     /* PWM #2 index */
    localparam PWM3            = 4'h3;     /* PWM #3 index */
    localparam PWM4            = 4'h4;     /* PWM #4 index */
    localparam PWM5            = 4'h5;     /* PWM #5 index */
    localparam PWM6            = 4'h6;     /* PWM #6 index */
    localparam PWM7            = 4'h7;     /* PWM #7 index */
    localparam PWM8            = 4'h8;     /* PWM #8 index */
    localparam PWM9            = 4'h9;     /* PWM #9 index */
    localparam PWM10           = 4'hA;     /* PWM #10 index */
    localparam PWM11           = 4'hB;     /* PWM #11 index */
    localparam PWM12           = 4'hC;     /* PWM #12 index */
    localparam PWM13           = 4'hD;     /* PWM #13 index */
    localparam PWM14           = 4'hE;     /* PWM #14 index */
    localparam PWM15           = 4'hF;     /* PWM #15 index */
    
    /* PWM Alert mask bits */
    localparam ALT_PWM0        = 4'h0;     /* PWM #0 Alert mask bit */
    localparam ALT_PWM1        = 4'h1;     /* PWM #1 Alert mask bit */
    localparam ALT_PWM2        = 4'h2;     /* PWM #2 Alert mask bit */
    localparam ALT_PWM3        = 4'h3;     /* PWM #3 Alert mask bit */
    localparam ALT_PWM4        = 4'h4;     /* PWM #4 Alert mask bit */
    localparam ALT_PWM5        = 4'h5;     /* PWM #5 Alert mask bit */
    localparam ALT_PWM6        = 4'h6;     /* PWM #6 Alert mask bit */
    localparam ALT_PWM7        = 4'h7;     /* PWM #7 Alert mask bit */
    localparam ALT_PWM8        = 4'h0;     /* PWM #8 Alert mask bit */
    localparam ALT_PWM9        = 4'h1;     /* PWM #9 Alert mask bit */
    localparam ALT_PWM10       = 4'h2;     /* PWM #10 Alert mask bit */
    localparam ALT_PWM11       = 4'h3;     /* PWM #11 Alert mask bit */
    localparam ALT_PWM12       = 4'h4;     /* PWM #12 Alert mask bit */
    localparam ALT_PWM13       = 4'h5;     /* PWM #13 Alert mask bit */
    localparam ALT_PWM14       = 4'h6;     /* PWM #14 Alert mask bit */
    localparam ALT_PWM15       = 4'h7;     /* PWM #15 Alert mask bit */
    
    localparam EOC             = 4'h0;     /* Defines EOC bit in DMA Control register */
    
    /* Fan Mode constants */
    localparam FW_MODE         = 1'b0;     /* Firmware controlled FAN mode (open loop) */
    localparam HW_MODE         = 1'b1;     /* UDB controlled FAN mode (closed loop) */
    
    /* UDB revisions */
    localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);
    
    /**************************************************************************
    * Internal Signals
    **************************************************************************/
    wire        sync_clock;                  /* Internal clock net synchronized to bus_clk */
    wire [7:0]  control;                     /* Global Control Register Output */
    wire [6:0]  status;                      /* Status Register Output */
    wire        alert_pin_en;                /* Alert Enable signal from control register */
    wire        stall_alert_en;              /* Stall Alert Enable signal from control register */
    wire        speed_alert_en;              /* Speed Alert Enable signal from control register */
    wire        stall_alert_clr;             /* Stall Alert Clear signal from control register */
    wire        speed_alert_clr;             /* Speed Alert Clear signal from control register */
    wire        enable;                      /* Enable signal from control register */
    wire        override;                    /* Override signal from control register */
    wire [15:0] speed_error;                 /* Speed error signal from closed loop PWMs */
    wire [7:0]  alert_mask_control_lsb;      /* Alert Mask Control Register LSB output */
    wire [7:0]  alert_mask_control_msb;      /* Alert Mask Control Register MSB output */
    wire [7:0]  stall_err_status_lsb;        /* Stall Error Status Register LSB input */
    wire [7:0]  stall_err_status_msb;        /* Stall Error Status Register MSB input */
    wire [7:0]  speed_err_status_lsb;        /* Stall Error Status Register LSB input */
    wire [7:0]  speed_err_status_msb;        /* Stall Error Status Register MSB input */
    wire        stall_status;                /* Stall status signal */
    wire        speed_status;                /* Speed status signal */
    wire        stall_alrt;                  /* Resulting Stall Alert signal. ORed with all all stall status' */
    wire        speed_alrt;                  /* Resulting Speed Alert signal. ORed with all all stall status' */
    reg         alert_reg;                   /* Registered version of alert output */
    wire        async_eoc;                   /* Asynchronous End-Of-Conversion pulse */
    wire        continuous_nrq;              /* Continuous nrq pulse that stays High until not cleared by async eoc */
    wire        async_nrq;                   /* Async version of continuous_nrq */
    wire        sync_nrq;                    /* continuous_nrq signal that is syncronized to component clock */
    
    /**************************************************************************
    * Clock Synchronization
    **************************************************************************/
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) FanClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(sync_clock)
    );  
    
    /**************************************************************************
    * Silicon depending blocks
    **************************************************************************/ 
    generate
    if (CY_UDB_V0)          /* PSoC3 ES2 and PSoC5 ES1 only */    
    begin: AsyncCtl
        
        /**************************************************************************
        * Global Control Register
        **************************************************************************/    
        cy_psoc3_control #(.cy_force_order(1))
        GlobalControlReg(
            /* output    [07:00]      */  .control(control)
        );
        
        /**************************************************************************
        * Alert Mask Control Register LSB
        **************************************************************************/    
        cy_psoc3_control #(.cy_force_order(1))
        AlertMaskLSB(
            /* output    [07:00]      */  .control(alert_mask_control_lsb)
        );
        
        if(NumOfFans > 4'h8)
        begin: CtrlAlertMSB
        
            /**************************************************************************
            * Alert Mask Control Register MSB
            **************************************************************************/    
            cy_psoc3_control #(.cy_force_order(1))
            AlertMaskMSB(
                /* output    [07:00]      */  .control(alert_mask_control_msb)
            );
        
        end

    end
    
    else                  /* PSoC3 ES3 only */    
    begin: SyncCtl
        
        /**************************************************************************
        * Global Control Register
        **************************************************************************/    
        cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hff))
        GlobalControlReg(
            /* input             */     .clock(sync_clock),
            /* output    [07:00] */     .control(control)
        );
        
        /**************************************************************************
        * Alert Mask Control Register LSB
        **************************************************************************/     
        cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hff))
        AlertMaskLSB(
            /* input             */     .clock(sync_clock),
            /* output    [07:00] */     .control(alert_mask_control_lsb)
        );

        if(NumOfFans > 4'h8)
        begin: CtrlAlertMSB
        
            /**************************************************************************
            * Alert Mask Control Register MSB
            **************************************************************************/     
            cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hff))
            AlertMaskMSB(
                /* input             */     .clock(sync_clock),
                /* output    [07:00] */     .control(alert_mask_control_msb)
            );
          
        end
        
    end
    endgenerate 
    
    /**************************************************************************
    * End-Of-Conversion pulse generation
    **************************************************************************/  
    
    /* This will generate an async continuous nrq signal that is cleared by ~sync_eoc */
    assign async_nrq = nrq | (~sync_nrq & continuous_nrq);
    
    cy_dff sync_dff1
    (
        .d(async_nrq), 
        .clk(bus_clock), 
        .q(continuous_nrq)
    );
    
    /* Sycnronize the nrq signal to component clock */
    cy_dff sync_dff2
    (
        .d(async_nrq), 
        .clk(sync_clock), 
        .q(sync_nrq)
    );
    
    assign async_eoc = ~sync_nrq & async_nrq;
    
    /* Sycnronize the eoc pulse */
    cy_dff sync_dff3
    (
        .d(async_eoc), 
        .clk(sync_clock), 
        .q(eoc)
    );
    
    /**************************************************************************
    * Global control reg signals assigment
    **************************************************************************/ 
    assign alert_pin_en = control[ALERT_PIN_EN]; 
    assign stall_alert_en = control[STALL_ALERT_EN];
    assign speed_alert_en = control[SPEED_ALERT_EN];
    assign stall_alert_clr = control[STALL_ALERT_CLR];
    assign speed_alert_clr = control[SPEED_ALERT_CLR];
    assign enable = control[ENABLE];
    assign override = control[OVERRIDE];
    
    /**************************************************************************
    * Closed loop PWMs mode
    **************************************************************************/
    generate 
    if (Mode == HW_MODE) 
    begin: CLOSED_LOOP
    
        /* Closed loop PWM #0 */
        B_ClosedLoopPWM_v2_0 #(.Address(4'h0), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_1
        (
            .clock(clock),
            .en(enable),
            .addr(addr),
            .speed_dn(spd_dn),
            .speed_up(spd_up),
            .speed_err_en(alt_mask[PWM0]),
            .pwm(pwm[PWM0]),
            .speed_err(speed_error[PWM0])
        );
        
        /* Speed status bit for FAN #0 */
        assign speed_err_status_lsb[ALT_PWM0] = speed_error[PWM0];
        
        if(NumOfFans >= 4'd2)
        begin: FAN2
            
            /* Closed loop PWM #1 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h1), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_2
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM1]),
                .pwm(pwm[PWM1]),
                .speed_err(speed_error[PWM1])
            );
            
            /* Speed status bit for FAN #1 */
            assign speed_err_status_lsb[ALT_PWM1] = speed_error[PWM1];
                    
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_lsb[ALT_PWM1] = 1'b0;
            assign pwm[PWM1] = 1'b0;
            assign speed_error[PWM1] = 1'b0;
        end
        
        if(NumOfFans >= 4'd3)
        begin: FAN3
            
            /* Closed loop PWM #2 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h2), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_3
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM2]),
                .pwm(pwm[PWM2]),
                .speed_err(speed_error[PWM2])
            );
            
            /* Speed status bit for FAN #2 */
            assign speed_err_status_lsb[ALT_PWM2] = speed_error[PWM2];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_lsb[ALT_PWM2] = 1'b0;
            assign pwm[PWM2] = 1'b0;
            assign speed_error[PWM2] = 1'b0;
        end

        if(NumOfFans >= 4'd4)
        begin: FAN4
            
            /* Closed loop PWM #3 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h3), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_4
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM3]),
                .pwm(pwm[PWM3]),
                .speed_err(speed_error[PWM3])
            );
            
            /* Speed status bit for FAN #3 */
            assign speed_err_status_lsb[ALT_PWM3] = speed_error[PWM3];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_lsb[ALT_PWM3] = 1'b0;
            assign pwm[PWM3] = 1'b0;
            assign speed_error[PWM3] = 1'b0;
        end

        if(NumOfFans >= 4'd5)
        begin: FAN5
            
            /* Closed loop PWM #4 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h4), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_5
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM4]),
                .pwm(pwm[PWM4]),
                .speed_err(speed_error[PWM4])
            );
            
            /* Speed status bit for FAN #4 */
            assign speed_err_status_lsb[ALT_PWM4] = speed_error[PWM4];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_lsb[ALT_PWM4] = 1'b0;
            assign pwm[PWM4] = 1'b0;
            assign speed_error[PWM4] = 1'b0;
        end

        if(NumOfFans >= 4'd6)
        begin: FAN6
            
            /* Closed loop PWM #5 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h5), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_6
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM5]),
                .pwm(pwm[PWM5]),
                .speed_err(speed_error[PWM5])
            );
            
            /* Speed status bit for FAN #5 */
            assign speed_err_status_lsb[ALT_PWM5] = speed_error[PWM5];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_lsb[ALT_PWM5] = 1'b0;
            assign pwm[PWM5] = 1'b0;
            assign speed_error[PWM5] = 1'b0;
        end

        if(NumOfFans >= 4'd7)
        begin: FAN7
            
            /* Closed loop PWM #6 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h6), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_7
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM6]),
                .pwm(pwm[PWM6]),
                .speed_err(speed_error[PWM6])
            );
            
            /* Speed status bit for FAN #6 */
            assign speed_err_status_lsb[ALT_PWM6] = speed_error[PWM6];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_lsb[ALT_PWM6] = 1'b0;
            assign pwm[PWM6] = 1'b0;
            assign speed_error[PWM6] = 1'b0;
        end

        if(NumOfFans >= 4'd8)
        begin: FAN8
            
            /* Closed loop PWM #7 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h7), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_8
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM7]),
                .pwm(pwm[PWM7]),
                .speed_err(speed_error[PWM7])
            );
            
            /* Speed status bit for FAN #7 */
            assign speed_err_status_lsb[ALT_PWM7] = speed_error[PWM7];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_lsb[ALT_PWM7] = 1'b0;
            assign pwm[PWM7] = 1'b0;
            assign speed_error[PWM7] = 1'b0;
        end

        if(NumOfFans >= 4'd9)
        begin: FAN9
            
            /* Closed loop PWM #8 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h8), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_9
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM8]),
                .pwm(pwm[PWM8]),
                .speed_err(speed_error[PWM8])
            );
            
            /* Speed status bit for FAN #8 */
            assign speed_err_status_msb[ALT_PWM8] = speed_error[PWM8];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_msb[ALT_PWM8] = 1'b0;
            assign pwm[PWM8] = 1'b0;
            assign speed_error[PWM8] = 1'b0;
        end

        if(NumOfFans >= 4'd10)
        begin: FAN10
            
            /* Closed loop PWM #9 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'h9), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_10
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM9]),
                .pwm(pwm[PWM9]),
                .speed_err(speed_error[PWM9])
            );
            
            /* Speed status bit for FAN #9 */
            assign speed_err_status_msb[ALT_PWM9] = speed_error[PWM9];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_msb[ALT_PWM9] = 1'b0;
            assign pwm[PWM9] = 1'b0;
            assign speed_error[PWM9] = 1'b0;
        end
        if(NumOfFans >= 4'd11)
        begin: FAN11
            
            /* Closed loop PWM #10 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'hA), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_11
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM10]),
                .pwm(pwm[PWM10]),
                .speed_err(speed_error[PWM10])
            );
            
            /* Speed status bit for FAN #10 */
            assign speed_err_status_msb[ALT_PWM10] = speed_error[PWM10];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_msb[ALT_PWM10] = 1'b0;
            assign pwm[PWM10] = 1'b0;
            assign speed_error[PWM10] = 1'b0;
        end
        
        if(NumOfFans >= 4'd12)
        begin: FAN12
            
            /* Closed loop PWM #11 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'hB), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_12
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM11]),
                .pwm(pwm[PWM11]),
                .speed_err(speed_error[PWM11])
            );
            
            /* Speed status bit for FAN #11 */
            assign speed_err_status_msb[ALT_PWM11] = speed_error[PWM11];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_msb[ALT_PWM11] = 1'b0;
            assign pwm[PWM11] = 1'b0;
            assign speed_error[PWM11] = 1'b0;
        end

        if(NumOfFans >= 4'd13)
        begin: FAN13
            
            /* Closed loop PWM #12 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'hC), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_13
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM12]),
                .pwm(pwm[PWM12]),
                .speed_err(speed_error[PWM12])
            );
            
            /* Speed status bit for FAN #12 */
            assign speed_err_status_msb[ALT_PWM12] = speed_error[PWM12];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_msb[ALT_PWM12] = 1'b0;
            assign pwm[PWM12] = 1'b0;
            assign speed_error[PWM12] = 1'b0;
        end

        if(NumOfFans >= 4'd14)
        begin: FAN14
            
            /* Closed loop PWM #13 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'hD), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_14
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM13]),
                .pwm(pwm[PWM13]),
                .speed_err(speed_error[PWM13])
            );
            
            /* Speed status bit for FAN #13 */
            assign speed_err_status_msb[ALT_PWM13] = speed_error[PWM13];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_msb[ALT_PWM13] = 1'b0;
            assign pwm[PWM13] = 1'b0;
            assign speed_error[PWM13] = 1'b0;
        end

        if(NumOfFans >= 4'd15)
        begin: FAN15
            
            /* Closed loop PWM #14 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'hE), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_15
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM14]),
                .pwm(pwm[PWM14]),
                .speed_err(speed_error[PWM14])
            );
            
            /* Speed status bit for FAN #14 */
            assign speed_err_status_msb[ALT_PWM14] = speed_error[PWM14];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_msb[ALT_PWM14] = 1'b0;
            assign pwm[PWM14] = 1'b0;
            assign speed_error[PWM14] = 1'b0;
        end

        if(NumOfFans >= 5'd16)
        begin: FAN16
            
            /* Closed loop PWM #15 */
            B_ClosedLoopPWM_v2_0 #(.Address(4'hF), .Resolution(Resolution), .ErrorCount(ErrorCount)) Fan_16
            (
                .clock(clock),
                .en(enable),
                .addr(addr),
                .speed_dn(spd_dn),
                .speed_up(spd_up),
                .speed_err_en(alt_mask[PWM15]),
                .pwm(pwm[PWM15]),
                .speed_err(speed_error[PWM15])
            );
            
            /* Speed status bit for FAN #15 */
            assign speed_err_status_msb[ALT_PWM15] = speed_error[PWM15];
            
        end
        else
        begin
            /* Handle floating connetions */
            assign speed_err_status_msb[ALT_PWM15] = 1'b0;
            assign pwm[PWM15] = 1'b0;
            assign speed_error[PWM15] = 1'b0;
        end
        
        /* Resulting FAN Speed Error signal */
        assign speed_alrt = speed_error[PWM0] | speed_error[PWM1] | speed_error[PWM2] | speed_error[PWM3] | 
                            speed_error[PWM4] | speed_error[PWM5] | speed_error[PWM6] | speed_error[PWM7] | 
                            speed_error[PWM8] | speed_error[PWM9] | speed_error[PWM10] | speed_error[PWM11] | 
                            speed_error[PWM12] | speed_error[PWM13] | speed_error[PWM14] | speed_error[PWM15];

        assign speed_status = speed_alrt & speed_alert_en;             /* To generate final speed alert status bit */
        
    end
    
    /**************************************************************************
    * Open Loop PWMs mode
    **************************************************************************/
    else
    begin: OPEN_LOOP
    
        /* Open loop PWM #0 */
        B_OpenLoopPWM_v2_0 #(.Resolution(Resolution)) FanPWM_1_2
        (
            .clock(clock),
            .en(enable),
            .pwm1(pwm[PWM0]),
            .pwm2(pwm[PWM1])
        );
        
        if(NumOfFans >= 4'd3)
        begin: FAN34
        
            /* Open loop PWM #1 */
            B_OpenLoopPWM_v2_0 #(.Resolution(Resolution)) FanPWM_3_4
            (
                .clock(clock),
                .en(enable),
                .pwm1(pwm[PWM2]),
                .pwm2(pwm[PWM3])
            );
        end
        else
        begin
            /* Handle floating connetions */
            assign pwm[PWM2] = 1'b0;
            assign pwm[PWM3] = 1'b0;
        end

        if(NumOfFans >= 4'd5)
        begin: FAN56
        
            /* Open loop PWM #2 */
            B_OpenLoopPWM_v2_0 #(.Resolution(Resolution)) FanPWM_5_6
            (
                .clock(clock),
                .en(enable),
                .pwm1(pwm[PWM4]),
                .pwm2(pwm[PWM5])
            );
        end
        else
        begin
            /* Handle floating connetions */
            assign pwm[PWM4] = 1'b0;
            assign pwm[PWM5] = 1'b0;
        end

        if(NumOfFans >= 4'd7)
        begin: FAN78
        
            /* Open loop PWM #3 */
            B_OpenLoopPWM_v2_0 #(.Resolution(Resolution)) FanPWM_7_8
            (
                .clock(clock),
                .en(enable),
                .pwm1(pwm[PWM6]),
                .pwm2(pwm[PWM7])
            );
        end
        else
        begin
            /* Handle floating connetions */
            assign pwm[PWM6] = 1'b0;
            assign pwm[PWM7] = 1'b0;
        end

        if(NumOfFans >= 4'd9)
        begin: FAN910
        
            /* Open loop PWM #4 */
            B_OpenLoopPWM_v2_0 #(.Resolution(Resolution)) FanPWM_9_10
            (
                .clock(clock),
                .en(enable),
                .pwm1(pwm[PWM8]),
                .pwm2(pwm[PWM9])
            );
        end
        else
        begin
            /* Handle floating connetions */
            assign pwm[PWM8] = 1'b0;
            assign pwm[PWM9] = 1'b0;
        end

        if(NumOfFans >= 4'd11)
        begin: FAN1112
        
            /* Open loop PWM #5 */
            B_OpenLoopPWM_v2_0 #(.Resolution(Resolution)) FanPWM_11_12
            (
                .clock(clock),
                .en(enable),
                .pwm1(pwm[PWM10]),
                .pwm2(pwm[PWM11])
            );
        end
        else
        begin
            /* Handle floating connetions */
            assign pwm[PWM10] = 1'b0;
            assign pwm[PWM11] = 1'b0;
        end

        if(NumOfFans >= 4'd13)
        begin: FAN1314
        
            /* Open loop PWM #6 */
            B_OpenLoopPWM_v2_0 #(.Resolution(Resolution)) FanPWM_13_14
            (
                .clock(clock),
                .en(enable),
                .pwm1(pwm[PWM12]),
                .pwm2(pwm[PWM13])
            );
        end
        else
        begin
            /* Handle floating connetions */
            assign pwm[PWM12] = 1'b0;
            assign pwm[PWM13] = 1'b0;
        end

        if(NumOfFans >= 4'd15)
        begin: FAN1516
        
            /* Open loop PWM #7 */
            B_OpenLoopPWM_v2_0 #(.Resolution(Resolution)) FanPWM_15_16
            (
                .clock(clock),
                .en(enable),
                .pwm1(pwm[PWM14]),
                .pwm2(pwm[PWM15])
            );
        end
        else
        begin
            /* Handle floating connetions */
            assign pwm[PWM14] = 1'b0;
            assign pwm[PWM15] = 1'b0;
        end

        assign speed_status = 1'b0;      /* In FW mode speed status is not used */ 
           
    end
    
    endgenerate
    
    /**************************************************************************
    * Stall Alert signals
    **************************************************************************/
     
    assign stall_err_status_lsb[ALT_PWM0] = stall[PWM0];    /* Stall status bit for FAN #0 */
    assign stall_err_status_lsb[ALT_PWM1] = stall[PWM1];    /* Stall status bit for FAN #1 */
    assign stall_err_status_lsb[ALT_PWM2] = stall[PWM2];    /* Stall status bit for FAN #2 */
    assign stall_err_status_lsb[ALT_PWM3] = stall[PWM3];    /* Stall status bit for FAN #3 */
    assign stall_err_status_lsb[ALT_PWM4] = stall[PWM4];    /* Stall status bit for FAN #4 */
    assign stall_err_status_lsb[ALT_PWM5] = stall[PWM5];    /* Stall status bit for FAN #5 */
    assign stall_err_status_lsb[ALT_PWM6] = stall[PWM6];    /* Stall status bit for FAN #6 */
    assign stall_err_status_lsb[ALT_PWM7] = stall[PWM7];    /* Stall status bit for FAN #7 */
    
    assign stall_err_status_msb[ALT_PWM8] = stall[PWM8];    /* Stall status bit for FAN #8 */
    assign stall_err_status_msb[ALT_PWM9] = stall[PWM9];    /* Stall status bit for FAN #9 */
    assign stall_err_status_msb[ALT_PWM10] = stall[PWM10];  /* Stall status bit for FAN #10 */
    assign stall_err_status_msb[ALT_PWM11] = stall[PWM11];  /* Stall status bit for FAN #11 */
    assign stall_err_status_msb[ALT_PWM12] = stall[PWM12];  /* Stall status bit for FAN #12 */
    assign stall_err_status_msb[ALT_PWM13] = stall[PWM13];  /* Stall status bit for FAN #13 */
    assign stall_err_status_msb[ALT_PWM14] = stall[PWM14];  /* Stall status bit for FAN #14 */
    assign stall_err_status_msb[ALT_PWM15] = stall[PWM15];  /* Stall status bit for FAN #15 */  
    
    /* Resulting FAN Stall Error signal */
    assign stall_alrt = stall[PWM0] | stall[PWM1] | stall[PWM2] | stall[PWM3] | stall[PWM4] | stall[PWM5] | 
                        stall[PWM6] | stall[PWM7] | stall[PWM8] | stall[PWM9] | stall[PWM10] | stall[PWM11] | 
                        stall[PWM12] | stall[PWM13] | stall[PWM14] | stall[PWM15];
    
    /**************************************************************************
    * Alert Generation Logic
    **************************************************************************/
    assign stall_status = stall_alrt & stall_alert_en;        /* To generate final stall alert status bit */
    
    /**************************************************************************
    * Statusi Register
    **************************************************************************/
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h03), .cy_int_mask(7'h7F)) AlertStatusReg
    (
        /* input         */ .clock(sync_clock),
        /* input [06:00] */ .status(status),
        /* output        */ .interrupt(interrupt)
    );
     
    assign status[0] = stall_status;
    assign status[1] = speed_status;
    assign status[2] = 1'b0;
    assign status[3] = 1'b0;
    assign status[4] = 1'b0;
    assign status[5] = 1'b0;
    assign status[6] = 1'b0;
    
    /**************************************************************************/
    /* Alert Generation Logic
    /**************************************************************************/
    always @(posedge sync_clock)
    begin
        /* Latch interrupt event synchronous to eoc pulse from FanTach hold it 
        * until cleared.
        */
        alert_reg <= interrupt & alert_pin_en & eoc;                 
    end
    
    assign alert = alert_reg; 
     
    /**************************************************************************
    * Stall Status Register LSB
    **************************************************************************/
    cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'hFF)) StallError_LSB 
    (
        /* input [07:00] */ .status(stall_err_status_lsb),
        /* input */         .reset(1'b0),
        /* input */         .clock(sync_clock)
    );

    /**************************************************************************
    * Stall Status Register MSB
    **************************************************************************/    
    generate 
    if(NumOfFans >= 4'd9) 
    begin: StallStatusMSB
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'hFF)) StallError_MSB 
        (
            /* input [07:00] */ .status(stall_err_status_msb),
            /* input */         .reset(1'b0),
            /* input */         .clock(sync_clock)
        );
    end
    endgenerate
    
    /* Speed Alert */
    generate
    if(Mode == HW_MODE)
    begin: SpeedAlrt

        /**************************************************************************
        * Speed Status Register LSB
        **************************************************************************/
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'hFF)) SpeedError_LSB 
        (
            /* input [07:00] */ .status(speed_err_status_lsb),
            /* input */         .reset(1'b0),
            /* input */         .clock(sync_clock)
        );

        /**************************************************************************
        * Speed Status Register MSB
        **************************************************************************/    
        if(NumOfFans >= 4'd9) 
        begin: MSB
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'hFF)) SpeedError_MSB 
            (
                /* input [07:00] */ .status(speed_err_status_msb),
                /* input */         .reset(1'b0),
                /* input */         .clock(sync_clock)
            );
        end
    
    end
    endgenerate
    
    /**************************************************************************
    * Component Outputs
    **************************************************************************/
    /* Assign alert masks ouputs with an Alert Mask Control register bits */
    assign alt_mask[PWM0] = alert_mask_control_lsb[ALT_PWM0];
    assign alt_mask[PWM1] = alert_mask_control_lsb[ALT_PWM1];
    assign alt_mask[PWM2] = alert_mask_control_lsb[ALT_PWM2];
    assign alt_mask[PWM3] = alert_mask_control_lsb[ALT_PWM3];
    assign alt_mask[PWM4] = alert_mask_control_lsb[ALT_PWM4];
    assign alt_mask[PWM5] = alert_mask_control_lsb[ALT_PWM5];
    assign alt_mask[PWM6] = alert_mask_control_lsb[ALT_PWM6];
    assign alt_mask[PWM7] = alert_mask_control_lsb[ALT_PWM7];
    
    /* The second Control Register is only available when NumOfFans > 8 */
    generate
    if(NumOfFans > 4'h8)
    begin
        assign alt_mask[PWM8] = alert_mask_control_msb[ALT_PWM8];
        assign alt_mask[PWM9] = alert_mask_control_msb[ALT_PWM9];
        assign alt_mask[PWM10] = alert_mask_control_msb[ALT_PWM10];
        assign alt_mask[PWM11] = alert_mask_control_msb[ALT_PWM11];
        assign alt_mask[PWM12] = alert_mask_control_msb[ALT_PWM12];
        assign alt_mask[PWM13] = alert_mask_control_msb[ALT_PWM13];
        assign alt_mask[PWM14] = alert_mask_control_msb[ALT_PWM14];
        assign alt_mask[PWM15] = alert_mask_control_msb[ALT_PWM15];
    end
    else
    begin
        assign alt_mask[PWM8] = 1'b0;
        assign alt_mask[PWM9] = 1'b0;
        assign alt_mask[PWM10] = 1'b0;
        assign alt_mask[PWM11] = 1'b0;
        assign alt_mask[PWM12] = 1'b0;
        assign alt_mask[PWM13] = 1'b0;
        assign alt_mask[PWM14] = 1'b0;
        assign alt_mask[PWM15] = 1'b0;
    end
    endgenerate
    
    assign en    = control[ENABLE];                 /* output signal of enable bit from Global Control register */
    assign ovrd  = control[OVERRIDE];               /* output signal of override bit from Global Control register */
    
endmodule
`endif /* B_FanController_v2_0_ALREADY_INCLUDED */


`ifdef B_ClosedLoopPWM_v2_0_ALREADY_INCLUDED
`else
`define B_ClosedLoopPWM_v2_0_ALREADY_INCLUDED
module B_ClosedLoopPWM_v2_0 
(
    input   wire            clock,          /* PWM clock. 6-24 MHz depending on resolution and fan PWM frequency */
    input   wire            en,             /* ClosedLoopPWM component enable */
    input   wire    [3:0]   addr,           /* Address bus from the FanTach component */
    input   wire            speed_dn,       /* Decrease PWM control signal from the FanTach component */
    input   wire            speed_up,       /* Increase PWM control signal from the FanTach component */
    input   wire            speed_err_en,   /* Speed regulation alert enable */
    output  reg             pwm,            /* PWM output for connection to fan */
    output  reg             speed_err       /* Speed regulation alert */
);
    
    /**************************************************************************
    * Parameters
    **************************************************************************/
    /* Customizer Parameters */
    parameter   Address    = 4'h0;          /* Address of this ClosedLoopPWM component */
    parameter   Resolution = 4'h0;          /* Resolution: 8-bit or 10-bit */
    parameter   ErrorCount = 7'h0;          /* Speed regulation error count before generating alert */

    /* PWM State Machine */
    localparam  PWM        = 2'b00;
    localparam  INC_DUTY   = 2'b01;
    localparam  DEC_DUTY   = 2'b10;
    localparam  HOLD_DUTY  = 2'b11;

    /**************************************************************************
    * Internal Signals
    **************************************************************************/
    /* Common to 8-bit and 16-bit PWM implementations */
    reg     [1:0]   pwm_state;              /* Closed loop PWM state machine */
    reg             pwm_up;                 /* Flag to adjust duty cycle up */
    reg             pwm_dn;                 /* Flag to adjust duty cycle down */
    reg             speed_rst;              /* Speed regulation error counter reset */
    reg             speed_en;               /* Speed regulation error counter enable */
    wire            synced_clock;           /* Internal clock net synchronized to bus_clk */
    wire            tc;                     /* Speed regulation error counter terminal count */
    wire            z1;                     /* Datapath flag: true when PWM down counter A1 == 0 */
    wire            z0;                     /* Datapath flag: true when PWM duty cycle compare A0 == 0 */
    wire            ce0;                    /* Datapath flag: true when PWM duty cycle compare A0 == period D0 */
    wire            cl1;                    /* Datapath flag: true when PWM down counter A1 < compare A0 */
    wire            ce1;                    /* Datapath flag: true when PWM down counter A1 == compare A0 */
    
    /* Unused datapath flags */
    wire    nc1;    
    wire    nc2;
    wire    nc3;
    wire    nc4;
    wire    nc5;

    /**************************************************************************
    * Clock Synchronization
    **************************************************************************/
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(synced_clock)
    );  

    /**************************************************************************
    * Closed Loop PWM State Machine
    **************************************************************************/
    always @ (posedge synced_clock)
    begin
        case (pwm_state)
            PWM:
                /* Make adjustments to PWM duty cycle only when current PWM period expired */
                if (z1)
                begin
                    /* If speed up requested and duty cycle not at max, then increment duty cycle */
                    if (pwm_up & !ce0)
                    begin
                        pwm_state <= INC_DUTY;
                    end
                    
                    /* If speed down requested and duty cycle not at zero, then decrement duty cycle */
                    else if (pwm_dn & !z0)
                    begin
                        pwm_state <= DEC_DUTY;
                    end
                    
                    /* No changes requested to fan speed, so do nothing */
                    else
                    begin
                        pwm_state <= HOLD_DUTY;
                    end
                end
                
            INC_DUTY:
                pwm_state <= PWM;

            DEC_DUTY:
                pwm_state <= PWM;
                
            HOLD_DUTY:
                pwm_state <= PWM;
                
            default:
                pwm_state <= PWM;
        endcase
    end

    /* Capture speed change regulation inputs from tach controller */
    always @ (posedge synced_clock)
    begin
        pwm_up <= ((addr==Address) & speed_up) |    /* Capture speed_up pulse from tach controller */
                   (pwm_up & !z1);                  /* Hold it until the state machine takes action */
                
        pwm_dn <= ((addr==Address) & speed_dn) |    /* Capture speed_dn pulse from tach controller */
                   (pwm_dn & !z1);                  /* Hold it until the state machine takes action */
    end

    /* Drive PWM output (A1=PWM down counter <= A0=duty cycle) */
    always @ (posedge synced_clock)
    begin
        pwm <= cl1 | ce0 | ~en;
    end

    /**************************************************************************
    * Speed Regulation Error Counter
    **************************************************************************/
    /* Common to 8-bit and 16-bit PWM implementations */
    cy_psoc3_count7 #(.cy_period(ErrorCount),.cy_route_ld(`TRUE),.cy_route_en(`TRUE),.cy_alt_mode(0)) SpeedErrorCounter
    (
        /*  input          */  .clock(synced_clock),
        /*  input          */  .reset(1'b0),
        /*  input          */  .load(speed_rst),
        /*  input          */  .enable(speed_en),
        /*  output [06:00] */  .count(),
        /*  output         */  .tc(tc)
    );

    /* Enable the counter only when the tach controller sends up or down pulses */
    always @ (posedge synced_clock)
    begin
        speed_en <= ((pwm_up | pwm_dn) & z1);
    end

    /* Reset the counter when up or down pulses are within duty cycle range */ 
    always @ (posedge synced_clock)
    begin
        if ((pwm_up & ~ce0) | (pwm_dn & ~z0))
        begin
            speed_rst <= 1'b1;
        end
        else
        begin
            speed_rst <= 1'b0;
        end
    end

    /* If counter reaches terminal count, flag speed regulation error */
    always @ (posedge synced_clock)
    begin
        speed_err <= (en & tc & speed_err_en);      /* Sticky latching is done in external status register */
    end
    
    /**************************************************************************
    * PWM Datapath
    **************************************************************************/
    generate 
    if (Resolution <= 8) 
    begin: PWM8
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0: PWM Mode - Dec A1*/
            `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1: Duty Cycle Increment - Inc A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2: Duty Cycle Decrement - Dec A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3: Duty Cycle Hold - Pass*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7: Not Used*/
            8'hFF, 8'h00,  /*CFG9:  */
            8'hFF, 8'hFF,  /*CFG11-10:  */
            `SC_CMPB_A1_D1, `SC_CMPA_A1_A0, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12:  */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_ALU, `SC_FIFO0_ALU,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14:  */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:  */
        }
        )) ClosedLoopFan8
        (
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(synced_clock),
                /*  input   [02:00]         */  .cs_addr({1'b0,pwm_state}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output                  */  .ce0(ce0),
                /*  output                  */  .cl0(),
                /*  output                  */  .z0(z0),
                /*  output                  */  .ff0(),
                /*  output                  */  .ce1(ce1),
                /*  output                  */  .cl1(cl1),
                /*  output                  */  .z1(z1),
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
    
    /* 10-bit PWM Implementation */
    begin: PWM10
        wire    nc1, nc2, nc3, nc4, nc5;    /* Unused datapath flags */
        
        cy_psoc3_dp16 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0: PWM Mode - Dec A1*/
            `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1: Duty Cycle Increment - Inc A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2: Duty Cycle Decrement - Dec A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3: Duty Cycle Hold - Pass*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7: Not Used*/
            8'hFF, 8'h00,  /*CFG9: */
            8'hFF, 8'hFF,  /*CFG11-10: */
            `SC_CMPB_A1_D1, `SC_CMPA_A1_A0, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12: */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14: */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16: */
        }
        ), .cy_dpconfig_b(
        {
            `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0: PWM Mode - Dec A1*/
            `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1: Duty Cycle Increment - Inc A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2: Duty Cycle Decrement - Dec A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3: Duty Cycle Hold - Pass*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7: Not Used*/
            8'hFF, 8'h00,  /*CFG9: */
            8'hFF, 8'hFF,  /*CFG11-10: */
            `SC_CMPB_A1_D1, `SC_CMPA_A1_A0, `SC_CI_B_CHAIN,
            `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN,
            `SC_SI_A_CHAIN, /*CFG13-12: Chain LSB Datapath*/
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
            `SC_FB_CHNED, `SC_CMP1_CHNED,
            `SC_CMP0_CHNED, /*CFG15-14: Chain LSB Datapath*/
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16: */
        }
        )) ClosedLoopFan10
        (
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(synced_clock),
                /*  input   [02:00]         */  .cs_addr({1'b0, pwm_state}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output  [01:00]         */  .ce0({ce0, nc1}),
                /*  output  [01:00]         */  .cl0(),
                /*  output  [01:00]         */  .z0({z0, nc2}),
                /*  output  [01:00]         */  .ff0(),
                /*  output  [01:00]         */  .ce1({ce1, nc3}),
                /*  output  [01:00]         */  .cl1({cl1, nc4}),
                /*  output  [01:00]         */  .z1({z1, nc5}),
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
`endif /* B_ClosedLoopPWM_v2_0_ALREADY_INCLUDED */


`ifdef B_OpenLoopPWM_v2_0_ALREADY_INCLUDED
`else
`define B_OpenLoopPWM_v2_0_ALREADY_INCLUDED
module B_OpenLoopPWM_v2_0 
(
    input   wire    clock,              /* PWM clock. 6-24 MHz depending on resolution and fan PWM frequency */
    input   wire    en,                 /* OpenLoopPWM component enable */
    output  reg     pwm1,               /* PWM 1 output */
    output  reg     pwm2                /* PWM 2 output */
);

    /**************************************************************************
    * Parameters
    **************************************************************************/
    /* Customizer Parameters */
    parameter   Resolution = 4'h0;      /* Resolution: 8-bit or 10-bit */

    /**************************************************************************
    * Internal Signals
    **************************************************************************/
    wire    synced_clock;               /* Internal clock net synchronized to bus_clk */
    wire    z0;                         /* Datapath flag: true when PWM down counter A0 == 0 */
    wire    ce0;                        /* Datapath flag: true when PWM down counter A0 == PWM1 duty cycle D0 */
    wire    cl0;                        /* Datapath flag: true when PWM down counter A0 <  PWM1 duty cycle D0 */
    wire    ce1;                        /* Datapath flag: true when PWM down counter A0 == PWM2 duty cycle D0 */
    wire    cl1;                        /* Datapath flag: true when PWM down counter A0 <  PWM2 duty cycle D0 */

    /**************************************************************************
    * Clock Synchronization
    **************************************************************************/
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(synced_clock)
    );  

    /**************************************************************************
    * PWM Outputs Driven Directly from Datapath Flags
    **************************************************************************/
    always @(posedge synced_clock)
    begin
        pwm1 <= cl0 | ce0 | ~en;        /* PWM logic is less than or equal. When disabled, force high to run fans
                                        * at max speed.
                                        */         
        pwm2 <= cl1 | ce1 | ~en;
    end
    
    /**************************************************************************
    * PWM Datapath
    **************************************************************************/
    generate 
    if (Resolution == 8) 
    begin: PWM8
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0: Decrement PWM Counter*/
            `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_A1,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1: Reload PWM Counter*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7: Not Used*/
            8'hFF, 8'h00,     /*CFG9: */
            8'hFF, 8'hFF,     /*CFG11-10: */
            `SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12: */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14: */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16: */
        })) OpenLoopFan8
        (
            /*  input                   */  .clk(synced_clock),
            /*  input   [02:00]         */  .cs_addr({2'b00,z0}),
            /*  input                   */  .route_si(1'b0),
            /*  input                   */  .route_ci(1'b0),
            /*  input                   */  .f0_load(1'b0),
            /*  input                   */  .f1_load(1'b0),
            /*  input                   */  .d0_load(z0),
            /*  input                   */  .d1_load(z0),
            /*  output                  */  .ce0(ce0),
            /*  output                  */  .cl0(cl0),
            /*  output                  */  .z0(z0),
            /*  output                  */  .ff0(),
            /*  output                  */  .ce1(ce1),
            /*  output                  */  .cl1(cl1),
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
    
    /* 10-bit PWM Implementation */
    begin: PWM10
        wire    nc1, nc2, nc3, nc4, nc5;    /* Unused datapath flags */

        cy_psoc3_dp16 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0: Decrement PWM Counter*/
            `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_A1,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1: Reload PWM Counter*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7: Not Used*/
            8'hFF, 8'h00,  /*CFG9: */
            8'hFF, 8'hFF,  /*CFG11-10: */
            `SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12: */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14: */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16: */
        }
        ), .cy_dpconfig_b(
        {
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0: Decrement PWM Counter*/
            `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_A1,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1: Reload PWM Counter*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6: Not Used*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7: Not Used*/
            8'hFF, 8'h00,  /*CFG9:   */
            8'hFF, 8'hFF,  /*CFG11-10:   */
            `SC_CMPB_A1_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN,
            `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN,
            `SC_SI_A_CHAIN, /*CFG13-12:   */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
            `SC_FB_CHNED, `SC_CMP1_CHNED,
            `SC_CMP0_CHNED, /*CFG15-14:   */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:   */
        }
        )) OpenLoopFan10
        (
                /*  input                   */  .clk(synced_clock),
                /*  input   [02:00]         */  .cs_addr({2'b00,z0}),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(z0),
                /*  input                   */  .d1_load(z0),
                /*  output  [01:00]         */  .ce0({ce0,nc1}),
                /*  output  [01:00]         */  .cl0({cl0,nc2}),
                /*  output  [01:00]         */  .z0({z0,nc3}),
                /*  output  [01:00]         */  .ff0(),
                /*  output  [01:00]         */  .ce1({ce1,nc4}),
                /*  output  [01:00]         */  .cl1({cl1,nc5}),
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
`endif /* B_OpenLoopPWM_v2_0_ALREADY_INCLUDED */
