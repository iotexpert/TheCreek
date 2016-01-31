/*******************************************************************************
 *
 * FILENAME:  B_Counter_v2_20.v
 * UM Name:   B_Counter_v2_20
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
* Copyright 2008-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef B_COUNTER_V_v2_20_ALREADY_INCLUDED
`else
`define B_COUNTER_V_v2_20_ALREADY_INCLUDED
module B_Counter_v2_20 (
    input    wire    reset,           /* system reset signal                   */
    input    wire    clock,           /* Synchronization clock                 */
    input    wire    count,           /* count input                           */
    input    wire    capture,         /* Capture trigger input                 */
    input    wire    enable,          /* Enable input                          */
    input    wire    up_ndown,        /* Up Not_Down direction input           */
    input    wire    upcnt,           /* Up count input                        */
    input    wire    dwncnt,          /* Down count input                      */
    output   wire    tc_out,          /* Terminal count output                 */
    output   wire    cmp_out,         /* Compare output                        */
    output   wire    irq_out          /* Interrupt request output signal       */
    );

    /* Internal signals */
    /* Period Compare Outputs */
    wire           per_zero;        /* A0 is equal to Zero                    */
    wire           per_equal;       /* A0 is equal to Period value            */
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
    wire           dp_dir;          /* Final Direction input to datapath      */


    /* Unused Datapath Output Signals */
    wire     nc1,nc2,nc3,nc4,nc5,nc6,nc7,nc8,nc9, nc10, nc11, nc12, nc13, nc14,
             nc15, nc16, nc17, nc18, nc19, nc20, nc21, nc22, nc23, nc24, nc25,
             nc26, nc27, nc28, nc29, nc30, nc31, nc32, nc33, nc34, nc35, nc36,
             nc37, nc38, nc39, nc40, nc41, nc42, nc43, nc44, nc45;

    /**************************************************************************/
    /* Parameters                                                             */
    /**************************************************************************/
    localparam [7:0] COUNTER_8_BIT  = 8'd8;
    localparam [7:0] COUNTER_16_BIT = 8'd16;
    localparam [7:0] COUNTER_24_BIT = 8'd24;
    localparam [7:0] COUNTER_32_BIT = 8'd32;
    parameter [7:0]  Resolution     = 8'd8;  /* default is 8-bit counter */

    localparam COUNTER_ENMODE_CRONLY = 2'b00;
    localparam COUNTER_ENMODE_HWONLY = 2'b01;
    localparam COUNTER_ENMODE_CR_HW  = 2'b10;
    parameter [1:0] EnableMode       = COUNTER_ENMODE_CRONLY;

    localparam COUNTER_RUNMODE_CONTINUOUS = 1'b0;
    localparam COUNTER_RUNMODE_ONESHOT    = 1'b1;
    parameter RunMode                     = COUNTER_RUNMODE_CONTINUOUS;

    localparam COUNTER_CM_Basic               = 2'd0;
    localparam COUNTER_CM_Clock_And_Direction = 2'd1;
    localparam COUNTER_CM_Clock_And_Up_Down   = 2'd2;
    parameter [1:0] ClockMode                 = COUNTER_CM_Basic;

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
    localparam COUNTER_CTRL_CMOD0    = 8'h0; /* Compare mode               */
    localparam COUNTER_CTRL_CMOD1    = 8'h1; /* Compare mode               */
    localparam COUNTER_CTRL_CMOD2    = 8'h2; /* Compare mode               */
    localparam COUNTER_CTRL_CAPMODE0 = 8'h3; /* Capture mode               */
    localparam COUNTER_CTRL_CAPMODE1 = 8'h4; /* Capture mode               */
    localparam COUNTER_CTRL_UNUSED   = 8'h6; /* Unused                     */
    localparam COUNTER_CTRL_ENABLE   = 8'h7; /* Enable Timer               */

    wire [7:0]  control;        /* Control Register Output    */
    wire        ctrl_enable;
    wire [2:0]  ctrl_cmod;
    wire [1:0]  ctrl_capmode;
    wire        overflow_status;
    wire        underflow_status;
    reg         overflow_reg_i;
    reg         underflow_reg_i;

    /***************************************************************************
     *          Device Family and Silicon Revision definitions                  *
    ***************************************************************************/
localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);

localparam CY_UDB_V1 = (!CY_UDB_V0);

    /* Clock Enable Block Component instantiation*/
    wire                ClockOutFromEnBlock;
    wire                final_enable;
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    clock_enable_block (
                        /* output */.clock_out(ClockOutFromEnBlock),
                        /* input */ .clock_in(clock),
                        /* input */ .enable(1'b1)
                        );

    /* Compare Mode Logic */
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

    /* Capture Mode Logic */
    generate
    if(CaptureMode == COUNTER_CAP_MODE_SW) begin : sCAPSW
        assign  ctrl_capmode =  {control[COUNTER_CTRL_CAPMODE1],
                                 control[COUNTER_CTRL_CAPMODE0]};
    end
    else begin
        assign ctrl_capmode = CaptureMode;
    end
    endgenerate

    /**************************************************************************
     * Control Register Implementation:
     * Use control register only when either of Enable Mode OR Compare Mode
     * Capture Mode is set to Software(with/without Hardware)
     **************************************************************************/
    generate
    if((EnableMode != COUNTER_ENMODE_HWONLY) || (CompareMode == COUNTER_CMP_MODE_SW) || (CaptureMode == COUNTER_CAP_MODE_SW)) begin : sCTRLReg
        /* Instantiate the control register */
        if(CY_UDB_V0)begin : AsyncCtl
            cy_psoc3_control #(.cy_force_order(`TRUE))
            ctrlreg(
                    /* output 07:00] */.control(control));
        end
        else begin : SyncCtl
            /* Add support of sync mode for PSoC3 Rev */
            /*******************************************************************
             * The clock to operate Control Reg for PSOC3 and PSOC5LP must be 
             * synchronous and run continuosly. In this case the 
             * udb_clock_enable is used only for synchronization. The resulted 
             * clock is always enabled.
             ******************************************************************/
            wire Clk_Ctl_i;
            cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
            cy_psoc3_udb_Ctl_Clk_Sync
                (
                 /* output */.clock_out(Clk_Ctl_i),
                 /* input */ .clock_in(clock),
                 /* input */ .enable(1'b1));

            cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFF))
            ctrlreg (
                /* input          */ .clock(Clk_Ctl_i),
                /* output [07:00] */ .control(control)
            );
        end
        assign ctrl_enable    = control[COUNTER_CTRL_ENABLE];
    end
    else begin
        assign ctrl_enable = 1'b1;
        assign control = 8'd0;
    end
    endgenerate

    /**************************************************************************/
    /*  Capture input implementation                                          */
    /**************************************************************************/
    wire      hwCapture;
    wire      capt_either_edge;
    wire      capt_rising;
    wire      capt_falling;
    reg       prevCapture;

    always @(posedge ClockOutFromEnBlock) begin
        prevCapture <= capture;
    end

    assign capt_rising = !prevCapture & capture;
    assign capt_falling = prevCapture & !capture;
    assign capt_either_edge = (capt_rising | capt_falling);

    assign hwCapture = (ctrl_capmode == COUNTER_CAP_MODE_NONE) ? 1'b0 :
                       (ctrl_capmode == COUNTER_CAP_MODE_RISE) ? capt_rising :
                       (ctrl_capmode == COUNTER_CAP_MODE_FALL) ? capt_falling :
                       /* (CaptureMode == COUNTER_CAP_MODE_BOTH) ?*/ capt_either_edge; 
    
    /**************************************************************************/
    /* Reload, Reset and Enable Implementations                               */
    /**************************************************************************/
    wire    reload;
    assign  reload = (ReloadOnReset & reset) |
                     (ReloadOnOverUnder & (overflow | underflow)) |
                     (ReloadOnCapture & hwCapture) |
                     (ReloadOnCompare & cmp_out);

    /* Enable handling */
    assign  final_enable = (EnableMode == COUNTER_ENMODE_CRONLY) ? ctrl_enable :
                           (EnableMode == COUNTER_ENMODE_HWONLY) ? enable :
                           /*(EnableMode == COUNTER_ENMODE_CR_HW) ?*/(ctrl_enable & enable);


    /**************************************************************************/
    /* RunMode Implementations                                                */
    /**************************************************************************/
    wire counter_enable;

    if(RunMode == COUNTER_RUNMODE_CONTINUOUS) begin
        assign counter_enable = final_enable;
    end
    else if(RunMode == COUNTER_RUNMODE_ONESHOT) begin
        reg disable_run_i;
        always @ (posedge ClockOutFromEnBlock) begin
            if (reset) begin
                disable_run_i <= 1'b0;
            end
            else if(overflow_status | underflow_status) begin
                disable_run_i <= 1'b1;
            end
        end
    assign counter_enable = final_enable & !disable_run_i;
    end

    /**************************************************************************/
    /* Status Register Implementation                                         */
    /**************************************************************************/
    localparam COUNTER_STS_CMP          = 8'h0; /* Compare output             */
    localparam COUNTER_STS_A0ZERO       = 8'h1; /* A0 Zero ouput              */
    localparam COUNTER_STS_OVERFLOW     = 8'h2; /* Overflow status            */
    localparam COUNTER_STS_UNDERFLOW    = 8'h3; /* Underflow                  */
    localparam COUNTER_STS_CAPTURE      = 8'h4; /* Capture Status             */
    localparam COUNTER_STS_FIFO_FULL    = 8'h5; /* FIFO Full Status           */
    localparam COUNTER_STS_FIFO_NEMPTY  = 8'h6; /* FIFO Not Empty Status      */


    generate
    if(UseInterrupt) begin : sSTSReg
        /* Status Register Input */
        wire   [6:0] status;       
        /* Edge Sensitive Compare implementation for Compare output */
        assign status[COUNTER_STS_CMP]       = cmp_out_status;   
        /* Already basically edge sense should be 1 clock cycle in most instances */
        assign status[COUNTER_STS_A0ZERO]    = per_zero;   
        /* Already basically edge sense should be 1 clock cycle in most instances */
        assign status[COUNTER_STS_OVERFLOW]  = overflow_status;  
        /* Already basically edge sense should be 1 clock cycle in most instances */
        assign status[COUNTER_STS_UNDERFLOW] = underflow_status; 
        /* Already implements edge sense to define hwCapture*/
        assign status[COUNTER_STS_CAPTURE]   = hwCapture;  
        /* Capture FIFO full status */
        assign status[COUNTER_STS_FIFO_FULL] = fifo_full;        
        /* Capture FIFO not empty status */
        assign status[COUNTER_STS_FIFO_NEMPTY] = fifo_nempty;    

        /* Instantiate the status register and interrupt hook*/
    if(CY_UDB_V1) begin :rstSts
            cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h1F),
                               .cy_int_mask(7'h7F))
            stsreg(
                   /* input          */  .clock(ClockOutFromEnBlock),
                   /* input  [06:00] */  .status(status),
                   /* output         */  .interrupt(irq_out),
                   /* input          */  .reset(reset)
                   );
    end
    else begin :nrstSts
            cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h1F),
                               .cy_int_mask(7'h7F))
            stsreg(
                   /* input          */  .clock(ClockOutFromEnBlock),
                   /* input  [06:00] */  .status(status),
                   /* output         */  .interrupt(irq_out)
               );
    end
    end
    endgenerate

    /**************************************************************************
    * Outputs
    /**************************************************************************/

    /***************************************************************************
     * Define Overflow and Underflow
     * a) For Basic Up/Down Counter, Overflow/Underflow occurs on the roll over
     *    at the terminal count of Period or Zero respectively.
     * b) For Clock with UpCnt-DownCnt, Overflow/Underflow occurs on the roll
     *    over at the terminal count of FF or Zero respectively.
     * c) For Clock with Direction, Overflow/Underflow occurs on the roll over
     *    at the terminal count of Period or Zero respectively.
     **************************************************************************/
    assign overflow = (ClockMode == COUNTER_CM_Clock_And_Up_Down) || (ClockMode == COUNTER_CM_Clock_And_Direction) ? per_FF : (dp_dir & per_equal);
    assign underflow = (ClockMode == COUNTER_CM_Clock_And_Up_Down) ||(ClockMode == COUNTER_CM_Clock_And_Direction)? per_zero : (!dp_dir & per_zero);

    /* Register and Pulse Logic for Overflow and Underflow to status register */
    always @( posedge ClockOutFromEnBlock) begin
        overflow_reg_i <= overflow;
        underflow_reg_i <= underflow;
    end
    assign overflow_status = overflow & !overflow_reg_i;
    assign underflow_status = underflow & !underflow_reg_i;

    /***************************************************************************
     * Terminal Count Output:
     * a) Up Counter : tc on TerminalCount = Period(Overflow)
     * b) Down Counter : tc on TerminalCount = Zero(Underflow)
     * c) Clock with UpCnt/DownCnt: tc on Terminal Count=Zero OR FFs (Underflow
     *    /Overflow)
     * d) Clock with Direction: tc on Terminal Count=Zero OR FFs (Underflow/
     *    Overflow
     **************************************************************************/
    generate
        wire tc_i;
        reg  tc_reg_i;
        assign tc_i = overflow | underflow;

        /* Register the TC output for avoiding glitches*/
        always @(posedge ClockOutFromEnBlock) begin
            tc_reg_i <= tc_i;
        end
        assign tc_out = tc_reg_i;
    endgenerate

    /**************************************************************************
    * Compare Output
    ***************************************************************************/
    wire cmp_out_i;
    reg prevCompare;
    reg cmp_out_reg_i;

    generate
    if(CompareMode == COUNTER_CMP_MODE_SW) begin : sCMPSW_OUTS
        assign cmp_out_i = (ctrl_cmod == COUNTER_CMP_MODE_LT)  ? cmp_less:
                           (ctrl_cmod == COUNTER_CMP_MODE_LTE) ? (cmp_less | cmp_equal):
                           (ctrl_cmod == COUNTER_CMP_MODE_EQ)  ? cmp_equal:
                           (ctrl_cmod == COUNTER_CMP_MODE_GT)  ? (!cmp_less & !cmp_equal):
                           /*(ctrl_cmod == COUNTER_CMP_MODE_GTE)?*/  !cmp_less;
    end
    else begin
        assign cmp_out_i = (CompareMode == COUNTER_CMP_MODE_LT)  ? cmp_less :
                           (CompareMode == COUNTER_CMP_MODE_LTE) ? (cmp_less | cmp_equal) :
                           (CompareMode == COUNTER_CMP_MODE_EQ)  ? cmp_equal :
                           (CompareMode == COUNTER_CMP_MODE_GT)  ? (!cmp_less & !cmp_equal):
                           /*(CompareMode == COUNTER_CMP_MODE_GTE)?*/  !cmp_less;
    end
    endgenerate

    generate
        if(CompareStatusEdgeSense == 1) begin : sCMPEdgeSense
            always @(posedge ClockOutFromEnBlock) begin
                prevCompare <= cmp_out_i;
            end
        /* Compare edge sense to status register */
        assign cmp_out_status = !prevCompare & cmp_out_i;
        end
        else begin
            assign cmp_out_status = cmp_out_i;
        end
    endgenerate

        /* Register compare edge sense to avoid glitch from comp o/p terminal*/
        always @(posedge ClockOutFromEnBlock) begin
        cmp_out_reg_i = cmp_out_i;
        end
        assign cmp_out = cmp_out_reg_i;

    /**************************************************************************/
    /* Up & Down Counter Implementation                                       */
    /**************************************************************************/
    wire dir_valid;
    reg  upcnt_stored;
    reg  dwncnt_stored;
    reg  count_up;
    reg  count_down;
    reg  count_stored_i;
    wire count_enable;

    generate
    if(ClockMode == COUNTER_CM_Clock_And_Up_Down) begin : UpDwn
        /* Up Count Edge Detect and Direction Trigger*/
        always @(posedge ClockOutFromEnBlock) begin
            if(upcnt) begin
                if(!upcnt_stored) begin
                    upcnt_stored <= 1'b1;
                    count_up <= 1'b1;
                end else begin
                    count_up <= 1'b0;
                end
            end else begin
                upcnt_stored <= 1'b0;
                count_up <= 1'b0;
            end
        end

        /* Down Count Edge Detect and Direction Trigger*/
        always @(posedge ClockOutFromEnBlock) begin
            if(dwncnt) begin
                if(!dwncnt_stored) begin
                    dwncnt_stored <= 1'b1;
                    count_down <= 1'b1;
                end else begin
                    count_down <= 1'b0;
                end  
            end else begin
                dwncnt_stored <= 1'b0;
                count_down <= 1'b0;
            end
        end

        /* direction valid and determination of count direction */
        assign count_enable = (count_up ^ count_down) & counter_enable;
        assign dp_dir = (upcnt | upcnt_stored) & (!upcnt_stored | !dwncnt | !dwncnt_stored);
    end else begin : UpORDownCounter 
        /* Edge Detect on Count input*/
        always @(posedge ClockOutFromEnBlock) begin
            count_stored_i <= count;
        end
        assign count_enable = (count & !count_stored_i) & counter_enable;
        assign dp_dir = up_ndown;
    end
    endgenerate

    /**************************************************************************/
    /* Instantiate the data path elements                                     */
    /**************************************************************************/
    wire [2:0] cs_addr = {dp_dir,count_enable,reload};

    parameter dpconfig0stdrd =
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG0 Comment:Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG1 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1 ) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG3 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG4 Comment:Idle */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG5 Comment:Preload Period (A0 <= ZERO)*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG6 Comment:Inc A0  ( A0 <= A0 + 1 ) */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG7 Comment:Preload Period (A0 <= ZERO)*/
          8'hFF, 8'h00,    /* SC_REG4    Comment: */
          8'hFF, 8'hFF,    /* SC_REG5    Comment: */
        `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /* SC_REG6 Comment: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
        1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /* SC_REG7 Comment: */
         10'h00, `SC_FIFO_CLK__DP/*`SC_FIFO_CLK_BUS*/,`SC_FIFO_CAP_FX,
        `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /* SC_REG8 Comment:G8    Comment */
    };

    parameter dpconfig1stdrd =
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG0 Comment:Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG1 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1 ) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG3 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG4 Comment:Idle */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG5 Comment:Preload Period (A0 <= ZERO) */
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG6 Comment:Inc A0  ( A0 <= A0 + 1 ) */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG7 Comment:Preload Period (A0 <= ZERO) */
          8'hFF, 8'h00, /* SC_REG4    Comment: */
          8'hFF, 8'hFF,    /* SC_REG5    Comment: */
        `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN,
        `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /* SC_REG6 Comment: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
        1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_CHNED, `SC_CMP1_CHNED,
        `SC_CMP0_CHNED, /* SC_REG7 Comment: */
         10'h00, `SC_FIFO_CLK__DP/*`SC_FIFO_CLK_BUS*/,`SC_FIFO_CAP_FX,
        `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL
    };

    parameter dpconfig0dir =
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG0 Comment:Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG1 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1 ) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG3 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG4 Comment:Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG5 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG6 Comment:Inc A0  ( A0 <= A0 + 1 ) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG7 Comment:Preload Period (A0 <= D0) */
          8'hFF, 8'h00,    /* SC_REG4    Comment: */
          8'hFF, 8'hFF, /* SC_REG5    Comment: */
        `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /* SC_REG6 Comment: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
        1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /* SC_REG7 Comment: */
         10'h00, `SC_FIFO_CLK__DP/*`SC_FIFO_CLK_BUS*/,`SC_FIFO_CAP_FX,
        `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /* SC_REG8 Comment:G8    Comment */
    };

    parameter dpconfig1dir =
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG0 Comment:Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG1 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG2 Comment:Dec A0  ( A0 <= A0 - 1 ) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG3 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG4 Comment:Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG5 Comment:Preload Period (A0 <= D0) */
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG6 Comment:Inc A0  ( A0 <= A0 + 1 ) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /* CS_REG7 Comment:Preload Period (A0 <= D0) */
          8'hFF, 8'h00, /* SC_REG4    Comment: */
          8'hFF, 8'hFF,    /* SC_REG5    Comment: */
        `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_CHAIN,
        `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /* SC_REG6 Comment: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
        1'b0, `SC_FIFO1_BUS, `SC_FIFO0__A0,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_CHNED, `SC_CMP1_CHNED,
        `SC_CMP0_CHNED, /* SC_REG7 Comment: */
         10'h00, `SC_FIFO_CLK__DP/*`SC_FIFO_CLK_BUS*/,`SC_FIFO_CAP_FX,
        `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL
    };


    parameter dpconfig0 = ((ClockMode == COUNTER_CM_Clock_And_Up_Down) || (ClockMode == COUNTER_CM_Clock_And_Direction)) ? dpconfig0dir : dpconfig0stdrd;
    parameter dpconfig1 = ((ClockMode == COUNTER_CM_Clock_And_Up_Down) || (ClockMode == COUNTER_CM_Clock_And_Direction)) ? dpconfig1dir : dpconfig1stdrd;

    generate
    if(Resolution == COUNTER_8_BIT) begin : sC8
    cy_psoc3_dp8 #(.cy_dpconfig_a(dpconfig0))
    counterdp(
    /*  input                   */  .clk(ClockOutFromEnBlock),
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),
    /*  input                   */  .route_ci(1'b0),
    /*  input                   */  .f0_load(hwCapture),
    /*  input                   */  .f1_load(1'b0),
    /*  input                   */  .d0_load(1'b0),
    /*  input                   */  .d1_load(1'b0),
    /*  output                  */  .ce0(per_equal),
    /*  output                  */  .cl0(nc42),
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
    end /* end of if statement for 8 bit section of generate */
    else if(Resolution <= COUNTER_16_BIT) begin : sC16
    cy_psoc3_dp16 #(.cy_dpconfig_a(dpconfig0), .cy_dpconfig_b(dpconfig1))
    counterdp(
    /*  input                   */  .clk(ClockOutFromEnBlock),
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),
    /*  input                   */  .route_ci(1'b0),
    /*  input                   */  .f0_load(hwCapture),
    /*  input                   */  .f1_load(1'b0),
    /*  input                   */  .d0_load(1'b0),
    /*  input                   */  .d1_load(1'b0),
    /*  output  [01:00]         */  .ce0({per_equal, nc16}),
    /*  output  [01:00]         */  .cl0({nc43, nc17}),
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
    /*  input                   */  .clk(ClockOutFromEnBlock),
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),
    /*  input                   */  .route_ci(1'b0),
    /*  input                   */  .f0_load(hwCapture),
    /*  input                   */  .f1_load(1'b0),
    /*  input                   */  .d0_load(1'b0),
    /*  input                   */  .d1_load(1'b0),
    /*  output  [02:00]         */  .ce0({per_equal,nc18,nc19}),
    /*  output  [02:00]         */  .cl0({nc44,nc20,nc21}),
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
    /*  input                   */  .clk(ClockOutFromEnBlock),
    /*  input   [02:00]         */  .cs_addr(cs_addr),
    /*  input                   */  .route_si(1'b0),
    /*  input                   */  .route_ci(1'b0),
    /*  input                   */  .f0_load(hwCapture),
    /*  input                   */  .f1_load(1'b0),
    /*  input                   */  .d0_load(1'b0),
    /*  input                   */  .d1_load(1'b0),
    /*  output  [03:00]         */  .ce0({per_equal,nc24,nc25,nc26}),
    /*  output  [03:00]         */  .cl0({nc45,nc27,nc28,nc29}),
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

`endif /*B_COUNTER_V_v2_20_ALREADY_INCLUDED*/
