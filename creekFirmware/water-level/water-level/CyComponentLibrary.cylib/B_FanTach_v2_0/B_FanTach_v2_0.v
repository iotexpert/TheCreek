/*******************************************************************************
*
* FILENAME:         B_FanTach_v2_0.v
* Component Name:   B_FanTach_v2_0
*
* DESCRIPTION:
*   The FanTach component is responsible for measuring the actual rotational
*   speeds of all fans in the system (up to 16) by measuring the period of the
*   fan's tachometer signal. In the event that the tachometer signal is not 
*   present, stuck low or high or toggling too slowly, this component generates
*   a stall alert for the affected fan.
*
*   In closed loop control mode, the FanTach block has the additional 
*   responsibility of comparing the measured speeds to system desired speeds 
*   and sending control signals to the Closed Loop fan driver PWM components in 
*   order to regulate fan speeds.
*
*   In order to minimize the loading on the CPU, DMA is used to transfer actual
*   fan speeds to an SRAM buffer, and fetch desired fan speeds from another SRAM
*   buffer. This autonomy allows the block to function properly without placing 
*   any real-time response requirements on the CPU.
*
*   Note that the tachometer speed measurement is hardcoded to support 4-pole
*   brushless DC (BLDC) fans that output two complete pulse trains per 
*   revolution of the fan. That is, two high periods plus two low period on the 
*   tachometer signal equals one rotation of the fan.
* 
*******************************************************************************
*                 Datapath Register Definitions (16-bit)
*******************************************************************************
*
*  INSTANCE NAME:  FanTachCounter 
*
*  DESCRIPTION:
*    This data path measures actual fan rotational speeds, compares them to 
*    desired speeds and generates control signals to the ClosedLoopPWMs.
*
*  REGISTER USAGE:
*    F0 => not used
*    F1 => not used
*    D0 => Desired fan tachometer period (inversely realted to desired speed)
*    D1 => Desired fan tachometer period hysteresis
*    A0 => Actual fan tachometer period
*    A1 => Fan period delta (Actual-Desired)
*
*  DATA PATH STATES:
*    0 0 0   0   IDLE: Reset tach period counter (A0<-0)
*    0 0 1   1   TACH_SYNC: Increment tach period counter, but check for Stall 
*                           (A0<-A0+1)
*    0 1 0   2   TACH_CLR: Found tach edge, reset tach period counter (A0<-0)
*    0 1 1   3   TACH_CNT1: Count 1st half of tach period (A0<-A0+1)
*    1 0 0   4   TACH_CNT2: Count 2nd half of tach period (A0<-A0+1)
*    1 0 1   5   COMPARE: Calculate speed delta  (A1<-A0-D0)
*    1 1 0   6   ACTION: Reset tach period counter (A0<-0)
*    1 1 1   7   STALL: Reset tach period counter (A0<-0)
*
*
*  INSTANCE NAME:  DmpgTimeCntr 
*
*  DESCRIPTION:
*    This data path operates as a 32-bit down counter to implement Damping 
*    Factor functionality.
*
*  REGISTER USAGE:
*    F0 => not used
*    F1 => not used
*    D0 => Low 16-bit Damping Factor period of 32 bit counter
*    D1 => High 16-bit Damping Factor period of 32 bit counter
*    A0 => Low 16-bit of accumulator of 32 bit counter
*    A1 => High 16-bit of accumulator of 32 bit counter
*
*  DATA PATH STATES:
*    0 0 0   0   IDLE: Reset Counter (A0<-D0, A1<-D1)
*    0 0 1   1   IDLE: Reset Counter (A0<-D0, A1<-D1)
*    0 1 0   2   IDLE: Reset Counter (A0<-D0, A1<-D1)
*    0 1 1   3   IDLE: Reset Counter (A0<-D0, A1<-D1)
*    1 0 0   4   DEC_A0: Decrement A0
*    1 0 1   5   DEC_A1: Decrement A1 and reload A0
*    1 1 0   6   DEC_A0: Decrement A0
*    1 1 1   7   IDLE: Reset Counter (A0<-D0, A1<-D1)
*
********************************************************************************
*                 I*O Signals:
********************************************************************************
*    Name              Direction       Description
*    tach_clock        input           500 kHz. Support fan speeds from 1,000 
*                                      -> 30,000 RPM
*    en                input           FanTach component enable
*    override          input           Signal that enabled CPU to override 
*                                      harwdare fan control
*    tach              input           Tachometer input from currently selected 
*                                      fan (external mux)
*    stl_mask[15:0]    input           Mask to enable/disable stall alert 
*                                      monitoring for each fan   
*    damping_clock     input           1 kHz clock to control damping factor of 
*                                      hardware control loop
*    addr[3:0]         output          Address bus to the input tachometer mux 
*                                      and ClosedLoopPWMs
*    speed_dn          output          Decrease PWM control signal to the 
*                                      ClosedLoopPWMs
*    speed_up          output          Increase PWM control signal to the 
*                                      ClosedLoopPWMs
*    stall[15:0]       output          Fan stall alert bit for each enabled fan
*    drq               output          DMA controller transfer request
*
********************************************************************************
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

`include "cypress.v"

`ifdef B_FanTach_v2_0_ALREADY_INCLUDED
`else
`define B_FanTach_v2_0_ALREADY_INCLUDED

module B_FanTach_v2_0 
(
    input   wire            tach_clock,     /* 500 kHz. Support fan speeds from 1,000->30,000 RPM */
    input   wire            en,             /* FanTach component enable */
    input   wire            override,       /* Signal that enabled CPU to override harwdare fan control */
    input   wire            tach,           /* Tachometer input from currently selected fan (external mux) */
    input   wire    [15:0]  stl_mask,       /* Mask to enable/disable stall alert monitoring for each fan */
    output  wire    [3:0]   addr,           /* Address bus to the input tachometer mux and ClosedLoopPWMs */
    output  reg             speed_dn,       /* Decrease PWM control signal to the ClosedLoopPWMs */
    output  reg             speed_up,       /* Increase PWM control signal to the ClosedLoopPWMs */
    output  wire    [15:0]  stall,          /* Fan stall alert bit for each enabled fan */
    output  reg             drq             /* DMA controller transfer request */
);

    /**************************************************************************
    * Parameters
    **************************************************************************/
    /* Customizer Parameters */
    parameter   NumberOfFans  = 5'h0;       /* 1 through 16 supported */
    parameter   ClosedLoop    = 1'b0;       /* Closed or open loop control mode */    
    parameter   DampingFactor = 7'h0;       /* Closed loop damping factor (1..127) */
    parameter   MotorType     = 1'b0;       /* Specifies the type of motor either 4-pole or six pole */
    
    localparam  FanCountPeriod = NumberOfFans - 1;

    /* FanTach Speed Counter State Machine - the state bits directly map to the 
    * datapath cs_addr.
    */
    localparam IDLE         = 4'b0000;     /* Reset tach period counter (A0->0) */
    localparam TACH_SYNC    = 4'b0001;     /* Increment tach period counter, but check for Stall (A0->A0+1) */
    localparam TACH_SYNC2   = 4'b1001;     /* Increment tach period counter, but check for Stall (A0->A0+1) */
    localparam TACH_CLR     = 4'b0010;     /* Found tach edge, reset tach period counter (A0->0) */
    localparam TACH_CLR2    = 4'b1010;     /* Found tach edge, reset tach period counter (A0->0) */
    localparam TACH_CNT1    = 4'b0011;     /* Count 1st half of tach period (A0->A0+1) */
    localparam TACH_CNT2    = 4'b0100;     /* Count 2nd half of tach period (A0->A0+1) */
    localparam TACH_CNT3    = 4'b1100;     /* Count 2nd half of tach period (A0->A0+1) */
    localparam COMPARE      = 4'b0101;     /* Calculate speed delta (A1=A0-D0)*/
    localparam ACTION       = 4'b0110;     /* Reset tach period counter (A0->0) */
    localparam STALL        = 4'b0111;     /* Reset tach period counter (A0->0) */

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

    localparam FOUR_POLE       = 1'b0;     /* Defines four pole motors */
    localparam SIX_POLE        = 1'b1;     /* Defines six pole motors */
    
    /**************************************************************************
    * Internal Signals
    **************************************************************************/
    genvar          i;                      /* Variable used by generate statements to instantiate arrays of logic */
    reg     [6:0]   glitch_filter;          /* Glitch filter for the tachometer input */
    reg             rising_tach;            /* True when rising tachometer edge found */
    reg             rising_tach_d;          /* True when rising tachometer edge found (delayed by 1 clock cycle) */
    reg     [3:0]   tach_state;             /* State machine that drives the datapath */
    reg             next_fan;               /* Control signal to advance the count7 block to address the next fan */
    reg             pulse_tc;               /* Indicates the end of speed measurement for specific fan fan */
    reg             stall_det;              /* Fan stall condition detected (tachometer counter overflow) */
    reg             reload;                 /* Control signal to reload the DampingFactor hardware timer */
    reg             glitch_filter_ld;       /* Load signal for the 7 bit glitch filter */
    reg     [15:0]  reg_stall;              /* Fan stall alert bit for each enabled fan */
    wire            filtered_rising_tach;   /* True when glitch filtered rising tachometer edge found */
    wire            glitch_filter_tc;       /* 7 bit glitch filter terminal count output*/
    wire            synced_tach_clock;      /* Internal clock net synchronized to bus_clk */
    wire    [1:0]   co;                     /* Dp flag: co  = tach counter overflow = stall condition */
    wire    [1:0]   cl0;                    /* Dp flag: cl0 = actual tach period < desired tach period */
    wire    [1:0]   ce0;                    /* Dp flag: ce0 = actual tach period = desired tach period */
    wire    [1:0]   cl1;                    /* Dp flag: cl1 = delta(actual tach period - desired tach period) 
                                            * < tolerance 
                                            */
    wire    [6:0]   fan_count;              /* Fan address generator */
    wire    [1:0]   damping_cntr_tc;        /* Terminal count output from the DampingFactor counter */
    wire    [1:0]   damping_cntr_reload_a0; /* Terminal count for 100 ms period of damping factor counter */
    wire            enable;                 /* Enable signal for Fan Tach state machine */
    reg             damping_q_fb;           /* Internal signal used in damping factor logic */
    reg             damping_nq;             /* Internal signal used in damping factor logic */
    wire            damping_factor_tc;      /* Final TC signal for DampingFactor counter */
    wire    [2:0]   damping_cntr_cs;        /* Enable signal for Fan Tach state machine */
    wire            end_of_measurement;     /* Indicates that speeed measurement complete for a set of fans */
    wire            dma_control;            /* Control Register used by DMA to generate eoc pulse */
    
    /* Not conected */
    wire            nc1;
    wire            nc2;
    
    /**************************************************************************
    * Clock Synchronization
    **************************************************************************/
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(tach_clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(synced_tach_clock)
    );
    
    /**************************************************************************
    * Fan/PWM Address Bus Generation                                         
    **************************************************************************/
    cy_psoc3_count7 #(.cy_period(FanCountPeriod),.cy_route_ld(`FALSE),.cy_route_en(`TRUE),.cy_alt_mode(`FALSE)) 
    FanCounter
    (
        /*  input          */  .clock(synced_tach_clock),
        /*  input          */  .reset(1'b0),
        /*  input          */  .load(1'b0),
        /*  input          */  .enable(next_fan),        /* Enabled by speed regulation control logic */
        /*  output [06:00] */  .count(fan_count),
        /*  output         */  .tc()
     );
    
    assign addr[3:0] = fan_count[3:0];
    
    /**************************************************************************
    * Closed Loop Control Mode Damping Factor Control                        
    *          
    * Designers can control the dynamic response time of the hardware control
    * loop using the DampingFactor customizer parameter. Increasing this     
    * parameter will just add delay to the fan control state machine in order
    * to prevent oscillations in the fan through over-agressive duty cycle   
    * adjustments. The fans need time to stabilize after duty cycle changes. 
    **************************************************************************/
    generate 
    if ((ClosedLoop == 1) && (DampingFactor > 0))
    begin: DmpgFactor
        
        assign damping_cntr_cs[0] = damping_cntr_reload_a0[1];
        assign damping_cntr_cs[1] = damping_cntr_tc[1];
        assign damping_cntr_cs[2] = en;
        
        if (NumberOfFans == 1)
        begin
            /* If we have only one Fan the end of speed measurement for fan #1 indicates 
            * the end of the speed measurement sequence.
            */
            assign end_of_measurement = next_fan;
        end
        else
        begin
            /* The condition when  
            */
            assign end_of_measurement = pulse_tc & !(fan_count[3] | fan_count[2] | fan_count[1] | fan_count[0]);
        end
        
        /**************************************************************************
        * Damping Factor Counter Datapath
        *
        * Damping Factor counter counts period of time specified by 'DampingFactor'
        * parameter. 'DampingFactor' is specified in tens of miliseconds.
        **************************************************************************/
        cy_psoc3_dp16 #(.cy_dpconfig_a(
        {
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4:     DEC_A0: Decrement A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5:     DEC_A1: Decrement A1 and reload A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6:     DEC_A0: Decrement A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            8'hFF, 8'h00,  /*CFG9:    */
            8'hFF, 8'hFF,  /*CFG11-10:    */
            `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
            `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
            `SC_SI_A_DEFSI, /*CFG13-12:    */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
            `SC_FB_NOCHN, `SC_CMP1_NOCHN,
            `SC_CMP0_NOCHN, /*CFG15-14:    */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:    */
        }
        ), .cy_dpconfig_b(
        {
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM0:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM1:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM2:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM3:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM4:     DEC_A0: Decrement A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC__ALU,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM5:     DEC_A1: Decrement A1 and reload A0*/
            `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM6:     DEC_A0: Decrement A0*/
            `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
            `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___D1,
            `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
            `CS_CMP_SEL_CFGA, /*CFGRAM7:     IDLE: Reset Counter (A0<-D0, A1<-D1)*/
            8'hFF, 8'h00,  /*CFG9:    */
            8'hFF, 8'hFF,  /*CFG11-10:    */
            `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_CHAIN,
            `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
            `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN,
            `SC_SI_A_CHAIN, /*CFG13-12:    */
            `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
            1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
            `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
            `SC_FB_CHNED, `SC_CMP1_CHNED,
            `SC_CMP0_CHNED, /*CFG15-14:    */
            10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
            `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
            `SC_WRK16CAT_DSBL /*CFG17-16:    */
        }
        )) DmpgTimeCntr
        (
                /*  input                   */  .reset(1'b0),
                /*  input                   */  .clk(synced_tach_clock),
                /*  input   [02:00]         */  .cs_addr(damping_cntr_cs),
                /*  input                   */  .route_si(1'b0),
                /*  input                   */  .route_ci(1'b0),
                /*  input                   */  .f0_load(1'b0),
                /*  input                   */  .f1_load(1'b0),
                /*  input                   */  .d0_load(1'b0),
                /*  input                   */  .d1_load(1'b0),
                /*  output  [01:00]         */  .ce0(),          
                /*  output  [01:00]         */  .cl0(),          
                /*  output  [01:00]         */  .z0(damping_cntr_reload_a0), /* TC for 10 ms period of Damping Factor
                                                                             * counter.
                                                                             */
                /*  output  [01:00]         */  .ff0(),
                /*  output  [01:00]         */  .ce1(),
                /*  output  [01:00]         */  .cl1(),              
                /*  output  [01:00]         */  .z1(damping_cntr_tc),        /* TC for Damping Factor counter that 
                                                                             * occures after DampingFactor*0.01 seconds.
                                                                             */
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
        
        /**************************************************************************
        * Signal damping_q_fb asserts high after damping the damping factor TC
        * pulses, which indicates that new speed measurement sequense for all
        * fan begins, this signal - damping_q_fb is acts as enable signal for 
        * current speed measurement sequence. When speed measurement sequence ends 
        * it asserts damping_q_fb low and with this it disables the following speed
        * measurement sequence till the next pulse of damping_factor_tc.
        *
        * Signal damping_nq asserts high in the case when the current speed 
        * measurement didn't finish yet but a new TC pulse of damping factor 
        * counter occured. So damping_nq indicates that speed measurement sequence
        * inserted a damping delay and new spead mesurement can start right after
        * the current suequence will end. 
        **************************************************************************/
        assign damping_factor_tc = damping_cntr_tc[1] & damping_cntr_reload_a0[1] & damping_cntr_reload_a0[0] & en;
        
        always@ (posedge synced_tach_clock) begin
       
            if(damping_factor_tc) begin            /* Starts new measurement sequence */
                if(damping_q_fb) begin
                    damping_nq <= 1'b1;            /* Indicates that delay inserted by speed measurement */
                end                                /* sequence is larger then a damping factor period. */ 
                else begin 
                    damping_q_fb <= 1'b1;
                    damping_nq <= 1'b0;
                end
            end
            else if(end_of_measurement) begin      /* Indicates that measurement sequence is complete */
                if(damping_nq) begin
                    damping_nq <= 1'b0;
                end
                else begin
                    damping_q_fb <= 1'b0;
                end
            end
            else begin
                /* damping_nq and damping_q_fb hold the previous state */
            end

        end
        
        /* Enale signal for Fan Tach state machine logic */
        assign enable = damping_q_fb;

    end
    
    /* If damping factor is not used */
    else begin
    
        /* Enale signal for Fan Tach state machine logic */
        assign enable = en;
        
    end
    
    endgenerate
    
    /**************************************************************************
    * Tachometer State Machine                                               
    **************************************************************************/
    /* Perform glitch filtering on rising edge of tachometer input using Count7 block
    * Each time unit = 2usec of filtering 
    */
    cy_psoc3_count7 #(.cy_period(120),.cy_route_ld(`TRUE),.cy_route_en(`FALSE),.cy_alt_mode(0)) GlitchFilterTimer
    (
        /*  input          */  .clock(synced_tach_clock),
        /*  input          */  .reset(1'b0),
        /*  input          */  .load(glitch_filter_ld),
        /*  input          */  .enable(1'b1),
        /*  output [06:00] */  .count(),
        /*  output         */  .tc(glitch_filter_tc)
    );

    /* Generate rising_tach pulse after filtering */
    always @ (posedge synced_tach_clock)
    begin
        glitch_filter_ld     <= !tach;                              /* Reload the glitch filter when the tach is low */
        rising_tach          <= (glitch_filter_tc) |                /* Glitch filter terminal count sets the flag */
                                (rising_tach & !glitch_filter_ld);  /* Hold the flag until the tach goes low */
        rising_tach_d        <= rising_tach;                        /* Delayed version for edge detection */
    end
    assign filtered_rising_tach = rising_tach & !rising_tach_d;     /* Pulses high for one clock cycle when filtered 
                                                                    * tach edge detected 
                                                                    */
    generate 
    if(MotorType == FOUR_POLE)
    begin
    
        /* State Machine Transitions */
        always @ (posedge synced_tach_clock)
        begin
            drq <=1'b0;
            case (tach_state)
            
                IDLE:                       /* Reset entry. Clear tach counter */
                    if (enable)
                    begin
                        tach_state <= TACH_SYNC;
                    end
                    
                TACH_SYNC:                  /* Ignore 1st rising edge as tach input switches. Stall if we timeout */
                    if (filtered_rising_tach) 
                    begin
                        tach_state <= TACH_CLR;
                    end
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end

                TACH_CLR:                       /* Got 1st rising edge. Reset counter to start actual measurement */
                    tach_state <= TACH_SYNC2;

                TACH_SYNC2:                     /* Wait for 1st real rising edge on selected tach input */
                    if (filtered_rising_tach)
                    begin
                        tach_state <= TACH_CLR2;
                    end
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end
                
                TACH_CLR2:                      /* Got 1st rising edge. Reset counter to start actual measurement */
                    tach_state <= TACH_CNT1;

                TACH_CNT1:                      /* Measure first tach pulse. Stall if we timeout */
                    if (filtered_rising_tach)
                    begin
                        tach_state <= TACH_CNT2;
                    end
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end

                TACH_CNT2:                      /* Measure 2nd tach pulse. Stall if we timeout */
                    if (filtered_rising_tach)
                    begin
                        tach_state <= COMPARE;
                        drq <= 1'b1;            /* Put actual speed, get next desired period and tolerance */
                    end    
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end

                COMPARE:                        /* Compare desired period to actual period using the datapath */
                        tach_state <= ACTION;

                ACTION:                         /* Take action depending on compare result */
                        tach_state <= IDLE;
                
                STALL:                          /* Stall condition detected */
                        tach_state <= IDLE;
                        
                default:
                    tach_state <= IDLE;
            endcase
        end
    end 
    else begin
    
         /* State Machine Transitions */
        always @ (posedge synced_tach_clock)
        begin
            drq <=1'b0;
            case (tach_state)
            
                IDLE:                       /* Reset entry. Clear tach counter */
                    if ((ClosedLoop == 1) && (DampingFactor > 0))
                    begin
                        if (enable)
                        begin
                            tach_state <= TACH_SYNC;
                        end
                    end
                    
                TACH_SYNC:                  /* Ignore 1st rising edge as tach input switches. Stall if we timeout */
                    if (filtered_rising_tach) 
                    begin
                        tach_state <= TACH_CLR;
                    end
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end

                TACH_CLR:                       /* Got 1st rising edge. Reset counter to start actual measurement */
                    tach_state <= TACH_SYNC2;

                TACH_SYNC2:                     /* Wait for 1st real rising edge on selected tach input */
                    if (filtered_rising_tach)
                    begin
                        tach_state <= TACH_CLR2;
                    end
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end
                
                TACH_CLR2:                      /* Got 2nd rising edge. Reset counter to start actual measurement */
                    tach_state <= TACH_CNT1;

                TACH_CNT1:                      /* Measure first tach pulse. Stall if we timeout */
                    if (filtered_rising_tach)
                    begin
                        tach_state <= TACH_CNT2;
                    end
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end

                TACH_CNT2:                      /* Measure 2nd tach pulse. Stall if we timeout */
                    if (filtered_rising_tach)
                    begin
                        tach_state <= TACH_CNT3;
                    end    
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end

                TACH_CNT3:                      /* Measure 3rd tach pulse. Stall if we timeout */
                    if (filtered_rising_tach)
                    begin
                        tach_state <= COMPARE;
                        drq <= 1'b1;            /* Put actual speed, get next desired period and tolerance */
                    end    
                    else if (co[1])
                    begin
                        tach_state <= STALL;
                        drq <= 1'b1;            /* Put speed=0 (stall), get next desired period and tolerance */
                    end                
                
                COMPARE:                        /* Compare desired period to actual period using the datapath */
                        tach_state <= ACTION;

                ACTION:                         /* Take action depending on compare result */
                        tach_state <= IDLE;
                
                STALL:                          /* Stall condition detected */
                        tach_state <= IDLE;
                        
                default:
                    tach_state <= IDLE;
            endcase
        end

    end
    endgenerate
    
    /**************************************************************************
    * Speed Regulation Control Logic                                         
    **************************************************************************/
    always @ (posedge synced_tach_clock)
    begin
        /* Valid tach measurement taken, decide what to do */
        if (tach_state == ACTION)
        begin
            stall_det <= 1'b0;                  /* Valid tach measurement, no stall */
            pulse_tc <= 1'b0;                   /* Set pulse_tc, that was risen on previous state, to low */
            next_fan  <= 1'b1;                  /* Change the address to the next fan using the count7 */
            if (ClosedLoop == 1)
            begin
                if (cl1[1])                     /* If delta (actual period - desired period) < tolerance */
                begin                           /* Then regulation achieved -> do nothing */
                end
                else if (~(cl0[1] | ce0[1]))    /* Else if actual period >= desired period */
                begin
                    speed_up <= ~override;      /* Then speed up (to reduce the actual period) */
                end
                else if (~cl1[1])               /* Else if delta (actual period - desired period) >= tolerance 
                                                * (includes delta is negative).
                                                */
                begin
                    speed_dn <= ~override;      /* Then slow down (to increase the actual period) */
                end
            end
        end    
        
        /* Tach measurement timed out -> stall condition */
        else if (tach_state == STALL)
        begin
            stall_det <= 1'b1;                  /* Flag the stall condition */
            next_fan  <= 1'b1;                  /* Change the address to the next fan */
            if (ClosedLoop == 1)
            begin
                speed_up  <= ~override;         /* Tell PWM to speed up to attempt recovery or re-start */
            end
        end
        
        /* State != ACTION and != STALL so do nothing */
        else if (tach_state == COMPARE)
        begin
            pulse_tc <= 1'b1;                   /* Indicate the speed measurement completion */
        end
        else
        begin 
            stall_det <= 1'b0;                  /* Valid tach measurement, no stall */
            next_fan  <= 1'b0;                  /* next_fan pulse is generated, set it low */
            speed_dn  <= 1'b0;                  /* speed_dn pulse is generated, set it low */
            speed_up  <= 1'b0;                  /* speed_up pulse is generated, set it low */
            pulse_tc  <= 1'b0;                  /* pulse_tc pulse is generated, set it low */
        end
    end    
   
    /* Generate stall alert if enabled */     
    generate
    for(i = 0; i < NumberOfFans; i = i + 1)
    begin
        always @ (posedge synced_tach_clock)
        begin
            if (i == addr)
            begin
                /* Sticky event latching done in external status registers */
                reg_stall[i] <= (en & stall_det & stl_mask[i]);   
            end
        end
    end
    endgenerate    
    
    /**************************************************************************
    * Handle stall[15..0] output assigment
    **************************************************************************/
    generate
    if(NumberOfFans < 4'd1)
    begin
        assign stall[PWM0] = 1'b0;
    end
    else
    begin
        assign stall[PWM0] = reg_stall[PWM0];
    end
    
    if(NumberOfFans < 4'd2)
    begin
        assign stall[PWM1] = 1'b0;
    end
    else
    begin
        assign stall[PWM1] = reg_stall[PWM1];
    end
    
    if(NumberOfFans < 4'd3)
    begin
        assign stall[PWM2] = 1'b0;
    end
    else
    begin
        assign stall[PWM2] = reg_stall[PWM2];
    end
    
    if(NumberOfFans < 4'd4)
    begin
        assign stall[PWM3] = 1'b0;
    end
    else
    begin
        assign stall[PWM3] = reg_stall[PWM3];
    end
    
    if(NumberOfFans < 4'd5)
    begin
        assign stall[PWM4] = 1'b0;
    end
    else
    begin
        assign stall[PWM4] = reg_stall[PWM4];
    end
    
    if(NumberOfFans < 4'd6)
    begin
        assign stall[PWM5] = 1'b0;
    end
    else
    begin
        assign stall[PWM5] = reg_stall[PWM5];
    end
    
    if(NumberOfFans < 4'd7)
    begin
        assign stall[PWM6] = 1'b0;
    end
    else
    begin
        assign stall[PWM6] = reg_stall[PWM6];
    end
    
    if(NumberOfFans < 4'd8)
    begin
        assign stall[PWM7] = 1'b0;
    end
    else
    begin
        assign stall[PWM7] = reg_stall[PWM7];
    end
    
    if(NumberOfFans < 4'd9)
    begin
        assign stall[PWM8] = 1'b0;
    end
    else
    begin
        assign stall[PWM8] = reg_stall[PWM8];
    end
    
    if(NumberOfFans < 4'd10)
    begin
        assign stall[PWM9] = 1'b0;
    end
    else
    begin
        assign stall[PWM9] = reg_stall[PWM9];
    end
    
    if(NumberOfFans < 4'd11)
    begin
        assign stall[PWM10] = 1'b0;
    end
    else
    begin
        assign stall[PWM10] = reg_stall[PWM10];
    end
    
    if(NumberOfFans < 4'd12)
    begin
        assign stall[PWM11] = 1'b0;
    end
    else
    begin
        assign stall[PWM11] = reg_stall[PWM11];
    end
    
    if(NumberOfFans < 4'd13)
    begin
        assign stall[PWM12] = 1'b0;
    end
    else
    begin
        assign stall[PWM12] = reg_stall[PWM12];
    end

    if(NumberOfFans < 4'd14)
    begin
        assign stall[PWM13] = 1'b0;
    end
    else
    begin
        assign stall[PWM13] = reg_stall[PWM13];
    end
    
    if(NumberOfFans < 4'd15)
    begin
        assign stall[PWM14] = 1'b0;
    end
    else
    begin
        assign stall[PWM14] = reg_stall[PWM14];
    end
    
    if(NumberOfFans < 5'd16)
    begin
        assign stall[PWM15] = 1'b0;
    end
    else
    begin
        assign stall[PWM15] = reg_stall[PWM15];
    end
    
    endgenerate
    
    /**************************************************************************
    * Tachometer Speed Measurement Datapath                                  
    **************************************************************************/
    cy_psoc3_dp16 #(.cy_dpconfig_a(
    {
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:       IDLE: Reset Counter*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:       TACH_SYNC: Check for Stall*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:       TACH_CLR: Reset Counter*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:       TACH_CNT1: Count 1st Half of Tach Period*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:       TACH_CNT2: Count 2nd Half of Tach Period*/
        `CS_ALU_OP__SUB, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:       COMPARE: A1=A0-D0*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:       ACTION: Reset Counter*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:       STALL: Reset Counter*/
        8'hFF, 8'h00,  /*CFG9:      */
        8'hFF, 8'hFF,  /*CFG11-10:      */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12:      */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
        1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:      */
        10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*CFG17-16:      */
    }
    ), .cy_dpconfig_b(
    {
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:       IDLE: Reset Counter*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:       TACH_SYNC: Check for Stall*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:       TACH_CLR: Reset Counter*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:       TACH_CNT1: Count 1st Half of Tach Period*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:       TACH_CNT2: Count 2nd Half of Tach Period*/
        `CS_ALU_OP__SUB, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:       COMPARE: A1=A0-D0*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:       ACTION: Reset Counter*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:       STALL: Reset Counter*/
        8'hFF, 8'h00,  /*CFG9:      */
        8'hFF, 8'hFF,  /*CFG11-10:      */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_CHAIN,
        `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_CHAIN,
        `SC_SI_A_CHAIN, /*CFG13-12:      */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
        1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_CHNED,
        `SC_FB_CHNED, `SC_CMP1_CHNED,
        `SC_CMP0_CHNED, /*CFG15-14:      */
        10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*CFG17-16:      */
    }
    )) FanTachCounter
    (
            /*  input                   */  .reset(1'b0),
            /*  input                   */  .clk(synced_tach_clock),
            /*  input   [02:00]         */  .cs_addr(tach_state[2:0]),
            /*  input                   */  .route_si(1'b0),
            /*  input                   */  .route_ci(1'b0),
            /*  input                   */  .f0_load(1'b0),
            /*  input                   */  .f1_load(1'b0),
            /*  input                   */  .d0_load(1'b0),
            /*  input                   */  .d1_load(1'b0),
            /*  output  [01:00]         */  .ce0(ce0),          /* Actual Period == Desired Period */
            /*  output  [01:00]         */  .cl0(cl0),          /* Actual Period  < Desired Period */
            /*  output  [01:00]         */  .z0(),
            /*  output  [01:00]         */  .ff0(),
            /*  output  [01:00]         */  .ce1(),
            /*  output  [01:00]         */  .cl1(cl1),          /* Delta (Actual Period - Desired Period) < Tolerance */
            /*  output  [01:00]         */  .z1(),
            /*  output  [01:00]         */  .ff1(),
            /*  output  [01:00]         */  .ov_msb(),
            /*  output  [01:00]         */  .co_msb(co),        /* Period measurement timed out */
            /*  output  [01:00]         */  .cmsb(),
            /*  output  [01:00]         */  .so(),
            /*  output  [01:00]         */  .f0_bus_stat(),
            /*  output  [01:00]         */  .f0_blk_stat(),
            /*  output  [01:00]         */  .f1_bus_stat(),
            /*  output  [01:00]         */  .f1_blk_stat()
    );
endmodule
`endif /* B_FanTach_v2_0_ALREADY_INCLUDED */
