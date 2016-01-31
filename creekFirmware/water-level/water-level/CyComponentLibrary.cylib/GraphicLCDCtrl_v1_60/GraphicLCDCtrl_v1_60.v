/*******************************************************************************
* File Name: GraphicLCDCtrl_v1_60.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* The component acts as a graphic LCD controller. It is designed to interface
* to an LCD panel that has an LCD driver, but that doesn't have an LCD 
* controller. This file provides a top level model of the GraphicLCDCtrl module
* defining the controller and datapath instances and all of the necessary
* interconnect
*
*------------------------------------------------------------------------------
*                 Control and Status Register definitions
*------------------------------------------------------------------------------
*    
*   Control Register Definition
*  +=======+-------+------+------+------+------+------+------+------+
*  |  Bit  |   7   |  6   |  5   |  4   |  3   |  2   |  1   |  0   |
*  +=======+-------+------+------+------+------+------+------+------+
*  | Desc  |                     unused                      |enable|
*  +=======+-------+------+------+------+------+------+------+------+
*
*    enable  =>   enable / ~reset component 
*    
*   Status Register Definition
*  +=======+-------+------+------+------+----------+----------+-------+-------+
*  |  Bit  |   7   |  6   |  5   |  4   |    3     |     2    |   1   |   0   |
*  +=======+-------+------+------+------+----------+----------+-------+-------+
*  | Desc  |           unused           |v_blanking|h_blanking| avail |  full |
*  +=======+-------+------+------+------+----------+----------+-------+-------+
*
*    full           =>  0 = there is a room in Command/Data FIFO    
*                       1 = Command/Data FIFO is full
*
*    avail          =>  0 = read operation has not completed
*                       1 = indicates that read data is available
*       
*   h_blanking      =>  0 = horizontal blanking interval is not going on
*                       1 = horizontal blanking interval is going on 
*   
*   v_blanking      =>  0 = vertical blanking interval is not going on
*                       1 = vertical blanking interval is going on 
*
********************************************************************************
* Data Path register definitions
********************************************************************************
* GraphicLCDCtrl: HorizDatapath and VertDatapath
* DESCRIPTION: Have the same configuration and register usage. Used to count the
*              four regions of the horizontal timing signals: 
*              Front Porch (FP), Sync, Back Porch (BP) and Active. 
* REGISTER USAGE:
* D0 - Front Porch (FP) counter (set one less than period).
* F0 - Back Porch (BP) counter (set 6 less than period).
* D1 - Sync pulse width counter (set one less than period).
* F1 - Active width counter (set as (period / 4) - 1).
* A0 - used to count for D0 and F0 sources.
* A1 - used to count for D1 and F1 sources.
*
********************************************************************************
* GraphicLCDCtrl: AddrDp0, AddrDp1, AddrDp2
* DESCRIPTION: Used as 24 bit wide datapath to generate addresses that are 
*              provided to an external async SRAM that implements the frame 
*              buffer to for panel refreshing or random SRAM access.
* REGISTER USAGE:
* A0 - Used to increment through the frame.  
* A1 - Used for random accesses.  
* D0 - Starting address for the entire frame of data.
* D1 - Increment at the end of a horizontal line.  
* F0 - Not used.
* F1 - Used to provide address and command information.  
*
********************************************************************************
* GraphicLCDCtrl: MsbDp and LsbDp
* DESCRIPTION: Used to implement 16 bit data bus for random SRAM access 
*              transactions.
* REGISTER USAGE:
* D0 - Not used.
* F0 - Used to provide output data during write.
* D1 - Not used.
* F1 - Used to latch input data during read.
* A0 - Used to latch data from parallel in or drive them to parallel out.
* A1 - Not used.
*
********************************************************************************
* I*O Signals:
********************************************************************************
*    input    wire         clock,      Clock that operates this component  
*    input    wire [7:0]   di_lsb,     Lower 8 bits of the input data bus  
*    input    wire [7:0]   di_msb,     Upper 8 bits of the input data bus  
*    output   wire [7:0]   addr0,      Lower 8 bits of the address bus     
*    output   wire [7:0]   addr1,      Middle 8 bits of the address bus    
*    output   wire [6:0]   addr2,      Upper 8 bits of the address bus     
*    output   wire         de,         Data enable for the panel     
*    output   wire [7:0]   do_lsb,     Lower 8 bits of the output data bus 
*    output   wire [7:0]   do_msb,     Upper 8 bits of the output data bus 
*    output   wire         doe,        OE for the data bus within PSoC     
*    output   wire         dotclk,     Clock driven to the panel            
*    output   wire         hsync,      Horizontal sync timing signal       
*    output   wire         vsync,      Vertical sync timing signal         
*    output   wire         noe,        Active low OE for the frame buffer  
*    output   wire         nwe,        Active low write enable signal      
*    output   wire         interrupt   Edge triggered interrupt signal                
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
*******************************************************************************/


`include "cypress.v"
`ifdef GraphicLCDCtrl_v1_60_V_ALREADY_INCLUDED
`else
`define GraphicLCDCtrl_v1_60_V_ALREADY_INCLUDED
module GraphicLCDCtrl_v1_60 
(
    input    wire         clock,      /* Clock that operates this component  */
    input    wire [7:0]   di_lsb,     /* Lower 8 bits of the input data bus  */
    input    wire [7:0]   di_msb,     /* Upper 8 bits of the input data bus  */
    output   wire [7:0]   addr0,      /* Lower 8 bits of the address bus     */
    output   wire [7:0]   addr1,      /* Middle 8 bits of the address bus    */
    output   wire [6:0]   addr2,      /* Upper 8 bits of the address bus     */
    output   wire [7:0]   do_lsb,     /* Lower 8 bits of the output data bus */
    output   wire [7:0]   do_msb,     /* Upper 8 bits of the output data bus */
    output   wire         interrupt,  /* Edge triggered interrupt signal     */
    output   reg          de,         /* Data enable for the panel           */
    output   reg          doe,        /* OE for the data bus within PSoC     */
    output   reg          dotclk,     /* Clock driven to the panel           */ 
    output   reg          hsync,      /* Horizontal sync timing signal       */
    output   reg          vsync,      /* Vertical sync timing signal         */
    output   reg          noe,        /* Active low OE for the frame buffer  */
    output   reg          nwe         /* Active low write enable signal      */
);

    /***************************************************************************
    *               Parameters                                                 *
    ***************************************************************************/
    parameter SyncPulsePolarityLow              = 1'b1;
    parameter TransitionDotclkFalling           = 1'b1;
    
    localparam INT_NONE                         = 2'd0;   
    localparam INT_ON_VERT_BLANKING             = 2'd1;   
    localparam INT_ON_VERT_AND_HORIZ_BLANKING   = 2'd2;
    
    parameter InterruptGen                      = INT_NONE;
    
    /***************************************************************************
    *            Device Family and Silicon Revision definitions 
    ***************************************************************************/   
    
    /* PSoC3 ES2 or earlier */
    localparam PSOC3_ES2  = ((`CYDEV_CHIP_MEMBER_USED   == `CYDEV_CHIP_MEMBER_3A) && 
                             (`CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_3A_ES2));
    /* PSoC5 ES1 or earlier */                        
    localparam PSOC5_ES1  = ((`CYDEV_CHIP_MEMBER_USED   == `CYDEV_CHIP_MEMBER_5A) && 
                             (`CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_5A_ES1));
    
                                       
    /***************************************************************************
    * Internal logical values for VSYNC and HSYNC
    * These signals are 1 during the pulse.  The polarity of the external
    * HSYNC and VSYNC signals depends on the setting for SyncPulsePolarityLow
    ***************************************************************************/
    wire vsyncInternal;
    wire hsyncInternal;

    /*************************************************************************** 
    * The upper bit from the address DP has a control signal that indicates 
    * read or write (read==1, write== 0).
    * That makes the maximum address 23 bits (addr2[6:0], addr1[7:0], addr0[7:0]
    ***************************************************************************/
    wire [7:0] addr2FromDP;
    assign addr2 = addr2FromDP[6:0];
    
    /***************************************************************************
    *                       Control register instantiation 
    ***************************************************************************/
    localparam GRAPH_LCD_CTL_ENABLE = 3'b0;  /* enable */
    localparam GRAPH_LCD_CTL_RESET  = 3'b1;  /* ~reset */
    wire [7:0] ctrl;
    wire reset;
    wire clock_enable;
        
    /* Add support of sync mode for PSoC3 ES3 Rev */
    generate
    if(PSOC3_ES2 || PSOC5_ES1)
    begin: AsyncCtl
        cy_psoc3_control #(.cy_force_order(1)) ControlReg
        (
            /* output [07:00] */  .control(ctrl)
        );
        assign reset = ~ctrl[GRAPH_LCD_CTL_ENABLE];
        assign clock_enable = 1'b1;
    end /* AsyncCtl */
    else
    begin: SyncCtl 
        /* The clock to operate Control Reg for ES3 must be synchronous and
        *  run continuosly. In this case the udb_clock_enable is used only 
        *  for synchronization. The resulted clock is always enabled.
        */ 
        cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) CtlClkSync
        (
            /* input  */    .clock_in(clock),
            /* input  */    .enable(1'b1),
            /* output */    .clock_out(ctl_clk)
        ); 
        /* control_0 - sync mode  */
        /* control_1 - pulse mode */
        cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h2), .cy_ctrl_mode_0(8'h3)) ControlReg
        (
            /*  input         */  .clock(ctl_clk),
            /* output [07:00] */  .control(ctrl)
        );
        assign reset        = ctrl[GRAPH_LCD_CTL_RESET];
        assign clock_enable = ctrl[GRAPH_LCD_CTL_ENABLE];
    end /* SyncCtl */
    endgenerate
    
    /***************************************************************************
    *         Instantiation of udb_clock_enable 
    ****************************************************************************
    * The udb_clock_enable primitive component allows to support clock enable 
    * mechanism and specify the intended synchronization behavior for the clock 
    * result (operational clock).
    */
    wire op_clk;
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(clock_enable),
        /* output */    .clock_out(op_clk)
    ); 
    
    /* Dotclock signal generation. Dotclock is half frequency of the incoming */ 
    /* clock. The register within the UDB array is used to divide the input   */
    /* frequency                                                              */
    reg dotclk_reg;
    always @(posedge op_clk)
    begin
        if(reset)
        begin
            dotclk_reg <= 1'b0;
        end
        else
        begin
            dotclk_reg <= ~dotclk_reg;
        end
    end     
    
    /***************************************************************************
    * FIFO full calculation is based on the upper address DP and the upper 
    * data DP.  The address DP FIFO status is used since the data DP is not 
    * used for reads.  The data DP is used since the data DP is read after the 
    * address DP so it can be full even with the address DP is no longer full.
    ***************************************************************************/
    wire notFullAddr2, notFullData1;
    wire full = ~(notFullAddr2 & notFullData1);    
    reg h_blanking;
    reg v_blanking;
    /* FIFO status indicating that a read value is available for the processor to get */
    wire avail;
    
    /* Status Register: 
    *  Bit 0: Used as an indication that the command/data fifos are full
    *  Bit 1: Used to indicate that read data is available 
    *  Bit 2: Used as an indication of horizontal blanking interval 
    *  Bit 3: Used as an indication of vertical blanking interval 
    */    
    cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h00)) StatusReg
    (
        /* input */             .clock(op_clk),
        /* input [07:00] */     .status({4'b0, v_blanking, h_blanking, avail, full})
    );

    /***************************************************************************
    *           REFRESH HORIZONTAL AND VERTICAL SIGNALING GENERATION            
    ***************************************************************************/
    
    /* Horizontal and vertical Datapath Configuration */
    localparam GRAPH_LCD_CTL_HV_DP_CONFIG = {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0: PASS */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1: COUNT0 */
        `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2: COUNT1 */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3: LOAD_FP */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___D1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4: LOAD_SYNC */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5: LOAD_BP */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6: LOAD_ACTIVE */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7: IDLE*/
        8'hFF, 8'h00,    /*CFG9:  */
        8'hFF, 8'hFF,    /*CFG11-10:  */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12:  */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
        1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:  */
        10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*CFG17-16:  */
    };
    
    /* Configuration Addresses for the H and V Datapaths */
    /* Both are programmed exactly the same */
    localparam GRAPH_LCD_CTL_HV_CFG_PASS        = 3'd0;
    localparam GRAPH_LCD_CTL_HV_CFG_COUNT0      = 3'd1;
    localparam GRAPH_LCD_CTL_HV_CFG_COUNT1      = 3'd2;
    localparam GRAPH_LCD_CTL_HV_CFG_LOAD_FP     = 3'd3;
    localparam GRAPH_LCD_CTL_HV_CFG_LOAD_SYNC   = 3'd4;
    localparam GRAPH_LCD_CTL_HV_CFG_LOAD_BP     = 3'd5;
    localparam GRAPH_LCD_CTL_HV_CFG_LOAD_ACTIVE = 3'd6;

    /***************************************************************************
    *              Horizontal Timing Generation
    ***************************************************************************/
    wire hz0;        /* Horizontal A0 count == 0 */
    wire hz1;        /* Horizontal A1 count == 0 */

    /***************************************************************************
    * Horizontal state machine
    * Moves to the next state as each counter counts down to 0
    * Special case for the ACTIVE state since that counts every 4 clocks.
    * ACTIVE state counts every 4 clocks in order to support up to 256*4 = 1024
    * wide horizontal active panels.
    * Reset state is at the beginning of the Front Porch (FP).
    *
    * D0 - Front Porch (FP) counter (set one less than period)
    * F0 - Back Porch (BP) counter (set 6 less than period)
    * D1 - Sync pulse width counter (set one less than period)
    * F1 - Active width counter (set as (period / 4) - 1)
    *    F0 and F1 used by holding the FIFO in clear mode
    *    Back Porch set 6 less than period because the BP is split into
    *    two pieces to keep random access from extending into the active
    *    period.  The longest random access takes 5 cycles:
    *       - BP: Back Porch where accesses can be started.
    *       - BP1 - BP5: Back Porch where accesses can not be started.
    *
    * A0 used to count for D0 and F0 sources.
    * A1 used to count for D1 and F1 sources.
    * 
    * State Machine has a fixed progression through the states as the counters
    * complete:
    *   FP - Front Porch
    *   SYNC - Sync pulse
    *   BP - Back Porch (Access allowed)
    *   BP1-BP5 - Back Porch (Access not allowed)
    *   ACTIVE - Active refresh area (implemented with 4 states to count at
    *                                 1/4th rate)
    ***************************************************************************/
    
    /* Horizontal State Machine States */
    localparam GRAPH_LCD_CTL_H_STATE_FP         = 4'd0;
    localparam GRAPH_LCD_CTL_H_STATE_SYNC       = 4'd1;
    localparam GRAPH_LCD_CTL_H_STATE_BP         = 4'd2;
    localparam GRAPH_LCD_CTL_H_STATE_BP1        = 4'd3;
    localparam GRAPH_LCD_CTL_H_STATE_BP2        = 4'd4;
    localparam GRAPH_LCD_CTL_H_STATE_BP3        = 4'd5;
    localparam GRAPH_LCD_CTL_H_STATE_BP4        = 4'd6;
    localparam GRAPH_LCD_CTL_H_STATE_BP5        = 4'd7;
    localparam GRAPH_LCD_CTL_H_STATE_ACTIVE1    = 4'd8;
    localparam GRAPH_LCD_CTL_H_STATE_ACTIVE2    = 4'd9;
    localparam GRAPH_LCD_CTL_H_STATE_ACTIVE3    = 4'd10;
    localparam GRAPH_LCD_CTL_H_STATE_ACTIVE4    = 4'd11;
    
    reg [3:0] hState;
    always @(posedge op_clk)
    begin
        if (reset) 
        begin
            hState <= GRAPH_LCD_CTL_H_STATE_FP;
        end
        else 
        begin
            if(dotclk_reg == TransitionDotclkFalling)
            begin
                case (hState)
                    GRAPH_LCD_CTL_H_STATE_FP: 
                        if (hz0) 
                        begin
                            hState <= GRAPH_LCD_CTL_H_STATE_SYNC;
                        end
                    GRAPH_LCD_CTL_H_STATE_SYNC: 
                        if (hz1) 
                        begin
                            hState <= GRAPH_LCD_CTL_H_STATE_BP;
                        end
                    GRAPH_LCD_CTL_H_STATE_BP: 
                        if (hz0) 
                        begin
                            hState <= GRAPH_LCD_CTL_H_STATE_BP1;
                        end
                    GRAPH_LCD_CTL_H_STATE_BP1: 
                        hState <= GRAPH_LCD_CTL_H_STATE_BP2;
                    GRAPH_LCD_CTL_H_STATE_BP2: 
                        hState <= GRAPH_LCD_CTL_H_STATE_BP3;
                    GRAPH_LCD_CTL_H_STATE_BP3: 
                        hState <= GRAPH_LCD_CTL_H_STATE_BP4;
                    GRAPH_LCD_CTL_H_STATE_BP4: 
                        hState <= GRAPH_LCD_CTL_H_STATE_BP5;
                    GRAPH_LCD_CTL_H_STATE_BP5: 
                        hState <= GRAPH_LCD_CTL_H_STATE_ACTIVE1;
                    GRAPH_LCD_CTL_H_STATE_ACTIVE1: 
                        hState <= GRAPH_LCD_CTL_H_STATE_ACTIVE2;
                    GRAPH_LCD_CTL_H_STATE_ACTIVE2: 
                        hState <= GRAPH_LCD_CTL_H_STATE_ACTIVE3;
                    GRAPH_LCD_CTL_H_STATE_ACTIVE3: 
                        hState <= GRAPH_LCD_CTL_H_STATE_ACTIVE4;
                    GRAPH_LCD_CTL_H_STATE_ACTIVE4: 
                        if (hz1) 
                        begin
                            hState <= GRAPH_LCD_CTL_H_STATE_FP;
                        end
                        else 
                        begin
                            hState <= GRAPH_LCD_CTL_H_STATE_ACTIVE1;
                        end
                    default: hState <= GRAPH_LCD_CTL_H_STATE_FP;
                endcase
            end
        end
    end
    
    /* Generation of addresses for the horizontal configuration memory */
    reg [2:0] hCfg;

    /* Horizontal Config Address generation */
    /* Load the next count value at the end of the previous count */
    always @(posedge op_clk)  
    begin
        if(dotclk_reg == TransitionDotclkFalling)
        begin
            case (hState)
                GRAPH_LCD_CTL_H_STATE_FP: 
                    if (hz0) 
                    begin
                        hCfg = GRAPH_LCD_CTL_HV_CFG_LOAD_SYNC;
                    end
                    else 
                    begin
                        hCfg = GRAPH_LCD_CTL_HV_CFG_COUNT0;
                    end
                GRAPH_LCD_CTL_H_STATE_SYNC: 
                    if (hz1) 
                    begin
                        hCfg = GRAPH_LCD_CTL_HV_CFG_LOAD_BP;
                    end
                    else 
                    begin
                        hCfg = GRAPH_LCD_CTL_HV_CFG_COUNT1;
                    end
                GRAPH_LCD_CTL_H_STATE_BP: 
                    if (hz0) 
                    begin
                        hCfg = GRAPH_LCD_CTL_HV_CFG_LOAD_ACTIVE;
                    end
                    else 
                    begin
                        hCfg = GRAPH_LCD_CTL_HV_CFG_COUNT0;
                    end
                GRAPH_LCD_CTL_H_STATE_ACTIVE4: 
                    if (hz1)
                    begin
                        hCfg = GRAPH_LCD_CTL_HV_CFG_LOAD_FP;
                    end
                    else 
                    begin
                        hCfg = GRAPH_LCD_CTL_HV_CFG_COUNT1;
                    end
                default: hCfg = GRAPH_LCD_CTL_HV_CFG_PASS;
            endcase
        end
        else
        begin
            hCfg = GRAPH_LCD_CTL_HV_CFG_PASS;
        end
    end

    /* Horizontal Datapath */
    cy_psoc3_dp8 #(.cy_dpconfig_a(GRAPH_LCD_CTL_HV_DP_CONFIG)) HorizDatapath
    (
        /*  input                   */  .clk(op_clk),
        /*  input   [02:00]         */  .cs_addr(hCfg),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(1'b0),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output                  */  .ce0(),
        /*  output                  */  .cl0(),
        /*  output                  */  .z0(hz0),
        /*  output                  */  .ff0(),
        /*  output                  */  .ce1(),
        /*  output                  */  .cl1(),
        /*  output                  */  .z1(hz1),
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
    
    /***************************************************************************
    *                     Vertical Timing Generation
    ***************************************************************************/
    wire vz0;        /* Vertical A0 count == 0 */
    wire vz1;        /* Vertical A1 count == 0 */
    
    /*************************************************************************** 
    * Vertical counting is done at the first cycle of the horizontal
    * front porch, so the skew of VSYNC before HSYNC is equal to the
    * horizontal front porch 
    ***************************************************************************/
    wire vCount; /* Only count the vertical counters once per horizontal line */
    reg endofline_dly;
    wire endofline = (hState == GRAPH_LCD_CTL_H_STATE_ACTIVE4) & hz1;
    always @(posedge op_clk)
    begin
        endofline_dly <= endofline;
    end
    assign vCount = endofline & endofline_dly;                                                                   

    /***************************************************************************
    * Vertical state machine is very similar to the horizontal state machine
    *
    * D0 - Front Porch (FP) counter (set one less than period)
    * F0 - Back Porch (BP) counter (set one less than period)
    * D1 - Sync pulse width counter (set one less than period)
    * F1 - Active width counter (set as (period / 4) - 1)
    *    F0 and F1 used by holding the FIFO in clear mode
    *
    * A0 used to count for D0 and F0 sources.
    * A1 used to count for D1 and F1 sources.
    *
    **************************************************************************** 
    
    /* Vertical State Machine States */
    localparam GRAPH_LCD_CTL_V_STATE_FP         = 3'd0;
    localparam GRAPH_LCD_CTL_V_STATE_SYNC       = 3'd1;
    localparam GRAPH_LCD_CTL_V_STATE_BP         = 3'd2;
    localparam GRAPH_LCD_CTL_V_STATE_ACTIVE1    = 3'd4;
    localparam GRAPH_LCD_CTL_V_STATE_ACTIVE2    = 3'd5;
    localparam GRAPH_LCD_CTL_V_STATE_ACTIVE3    = 3'd6;
    localparam GRAPH_LCD_CTL_V_STATE_ACTIVE4    = 3'd7;
    
    reg [2:0] vState;
    always @(posedge op_clk) 
    begin
        if (reset) 
        begin
            vState <= GRAPH_LCD_CTL_V_STATE_FP;
        end
        else 
        begin
            if (vCount) 
            begin
                case (vState)
                    GRAPH_LCD_CTL_V_STATE_FP: 
                        if (vz0) 
                        begin
                            vState <= GRAPH_LCD_CTL_V_STATE_SYNC;
                        end    
                    GRAPH_LCD_CTL_V_STATE_SYNC: 
                        if (vz1) 
                        begin
                            vState <= GRAPH_LCD_CTL_V_STATE_BP;
                        end
                    GRAPH_LCD_CTL_V_STATE_BP: 
                        if (vz0) 
                        begin
                            vState <= GRAPH_LCD_CTL_V_STATE_ACTIVE1;
                        end
                    GRAPH_LCD_CTL_V_STATE_ACTIVE1: 
                        vState <= GRAPH_LCD_CTL_V_STATE_ACTIVE2;
                    GRAPH_LCD_CTL_V_STATE_ACTIVE2: 
                        vState <= GRAPH_LCD_CTL_V_STATE_ACTIVE3;
                    GRAPH_LCD_CTL_V_STATE_ACTIVE3: 
                        vState <= GRAPH_LCD_CTL_V_STATE_ACTIVE4;
                    GRAPH_LCD_CTL_V_STATE_ACTIVE4: 
                        if (vz1)
                        begin
                            vState <= GRAPH_LCD_CTL_V_STATE_FP;
                        end
                        else 
                        begin
                            vState <= GRAPH_LCD_CTL_V_STATE_ACTIVE1;
                        end
                    default: vState <= GRAPH_LCD_CTL_V_STATE_FP;
                endcase
            end
        end
    end
    
    /* Generation of addresses for the vertical configuration memory */
    /* Uses the same configuration memory values as the horizontal DP */
    reg [2:0] vCfg;
    
    /* Vertical Config Address generation */
    /* Load the next count value at the end of the previous count */
    always @(posedge op_clk) 
    begin
        if (!vCount) 
        begin
            vCfg = GRAPH_LCD_CTL_HV_CFG_PASS;
        end
        else 
        begin
            case (vState)
                GRAPH_LCD_CTL_V_STATE_FP: 
                    if (vz0) 
                    begin
                        vCfg = GRAPH_LCD_CTL_HV_CFG_LOAD_SYNC;
                    end
                    else 
                    begin
                        vCfg = GRAPH_LCD_CTL_HV_CFG_COUNT0;
                    end
                GRAPH_LCD_CTL_V_STATE_SYNC: 
                    if (vz1) 
                    begin
                        vCfg = GRAPH_LCD_CTL_HV_CFG_LOAD_BP;
                    end
                    else 
                    begin
                        vCfg = GRAPH_LCD_CTL_HV_CFG_COUNT1;
                    end
                GRAPH_LCD_CTL_V_STATE_BP: 
                    if (vz0) 
                    begin
                        vCfg = GRAPH_LCD_CTL_HV_CFG_LOAD_ACTIVE;
                    end
                    else 
                    begin
                        vCfg = GRAPH_LCD_CTL_HV_CFG_COUNT0;
                    end
                GRAPH_LCD_CTL_V_STATE_ACTIVE4: 
                    if (vz1) 
                    begin
                        vCfg = GRAPH_LCD_CTL_HV_CFG_LOAD_FP;
                    end
                    else 
                    begin
                        vCfg = GRAPH_LCD_CTL_HV_CFG_COUNT1;
                    end
                default: vCfg = GRAPH_LCD_CTL_HV_CFG_PASS;
            endcase
        end
    end

    /* Vertical Datapath */
    cy_psoc3_dp8 #(.cy_dpconfig_a(GRAPH_LCD_CTL_HV_DP_CONFIG)) VertDatapath
    (
        /*  input                   */  .clk(op_clk),
        /*  input   [02:00]         */  .cs_addr(vCfg),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(1'b0),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output                  */  .ce0(),
        /*  output                  */  .cl0(),
        /*  output                  */  .z0(vz0),
        /*  output                  */  .ff0(),
        /*  output                  */  .ce1(),
        /*  output                  */  .cl1(),
        /*  output                  */  .z1(vz1),
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
    
    /***************************************************************************
    *            Timing Signals for Horizontal and Vertical timing
    ***************************************************************************/

    /* SYNCs active during the SYNC state */
    /* HSYNC/VSYNC polarity selected by a parameter on the component */
    assign hsyncInternal = (hState == GRAPH_LCD_CTL_H_STATE_SYNC);
    always @(posedge op_clk)
    begin
        hsync <= hsyncInternal ^ SyncPulsePolarityLow;
    end

    assign vsyncInternal = (vState == GRAPH_LCD_CTL_V_STATE_SYNC);
    always @(posedge op_clk)
    begin
        vsync <= vsyncInternal ^ SyncPulsePolarityLow;
    end
                       
    /* Data enabled when both horizontal and vertical are in the active state */
    always @(posedge op_clk)
    begin
        de <= hState[3] & vState[2];
    end
 
    /* Dotclock is the half frequency of the incoming clock */ 
    always @(posedge op_clk)
    begin
        dotclk <= dotclk_reg;
    end

    /***************************************************************************
    *                           Read and Write Access Generation
    ***************************************************************************/ 
    /* Determine when an access can be started to the RAM
    * Access can be started during the vertical or horizontal blanking interval
    * except for the end of the horizontal blanking interval (BP1-5).
    * The special case of the end of the vertical blanking interval is handled
    * properly since the vertical state transitions at the beginning of the 
    * horizontal front porch. */

    /* Horizontal blanking intervals */
    always @(posedge op_clk)
    begin
        h_blanking <= (hState == GRAPH_LCD_CTL_H_STATE_FP)   || 
                      (hState == GRAPH_LCD_CTL_H_STATE_SYNC) ||
                      (hState == GRAPH_LCD_CTL_H_STATE_BP);
    end                 
    /* Vertical blanking intervals */
    always @(posedge op_clk)
    begin
        v_blanking <= (vState == GRAPH_LCD_CTL_V_STATE_FP)   || 
                      (vState == GRAPH_LCD_CTL_V_STATE_SYNC) ||
                      (vState == GRAPH_LCD_CTL_V_STATE_BP);
    end
    
    wire acc;       /* High when an access to RAM can be initiated */
    
    assign acc = h_blanking | v_blanking;
    
    wire cmdFifoEmpty;  /*  High when the command FIFO is empty */
                        /*  This is the block status from the upper byte of the address/cmd */
                        /*  so this byte must be written last after the lower address bits and */
                        /*  the data in the case of a write. */
    wire cmdRead;       /*  High when the command is a Read */
                        /*  This is the upper bit of the 24 bit address.  (Read = 1, Write = 0) */
    assign cmdRead = addr2FromDP[7];
      
    /***************************************************************************
    * Access State Machine:
    *  Stay in the NONE state until a command is received in the address FIFOs
    *  Once a command is received progress through either the read or write states
    *  States for a write:
    *    NONE -> LOAD -> DECODE -> WRITE1 -> WRITE2 -> WRITE3
    *  States for a read:
    *    NONE -> LOAD -> DECODE -> READ
    *
    *  A read should be possible with just a single READ state instead of 2.  
    *  The need for the second read state is being investigated.  If the read 
    *  data is captured during the READ1 state the data captured is the wrong data.
    *
    ***************************************************************************/

    /* Access State Machine States */
    localparam GRAPH_LCD_CTL_ACCESS_STATE_NONE      = 3'd0;     /* No access                */
    localparam GRAPH_LCD_CTL_ACCESS_STATE_LOAD      = 3'd1;     /* Load a command           */
    localparam GRAPH_LCD_CTL_ACCESS_STATE_DECODE    = 3'd3;     /* Decode a command         */
    localparam GRAPH_LCD_CTL_ACCESS_STATE_WRITE1    = 3'd2;     /* Start the write with WE  */
    localparam GRAPH_LCD_CTL_ACCESS_STATE_WRITE2    = 3'd6;     /* Latch data into the RAM  */
    localparam GRAPH_LCD_CTL_ACCESS_STATE_WRITE3    = 3'd7;     /* Disable the data driver  */
    localparam GRAPH_LCD_CTL_ACCESS_STATE_READ      = 3'd5;     /* Latch the read data      */ 
    
    reg [2:0] accessState;
    always @(posedge op_clk) 
    begin
        if (reset) 
        begin
            accessState <= GRAPH_LCD_CTL_ACCESS_STATE_NONE;
        end 
        else 
        begin
            if(dotclk_reg == TransitionDotclkFalling)
            begin
                case (accessState)
                    GRAPH_LCD_CTL_ACCESS_STATE_NONE: 
                        if (acc & ~cmdFifoEmpty) 
                        begin
                            accessState <= GRAPH_LCD_CTL_ACCESS_STATE_LOAD;
                        end
                        else 
                        begin
                            accessState <= GRAPH_LCD_CTL_ACCESS_STATE_NONE;
                        end
                    GRAPH_LCD_CTL_ACCESS_STATE_LOAD: 
                        accessState <= GRAPH_LCD_CTL_ACCESS_STATE_DECODE;
                    GRAPH_LCD_CTL_ACCESS_STATE_DECODE: 
                        if (cmdRead) 
                        begin
                            accessState <= GRAPH_LCD_CTL_ACCESS_STATE_READ;                            
                        end
                        else 
                        begin
                            accessState <= GRAPH_LCD_CTL_ACCESS_STATE_WRITE1;
                        end
                    GRAPH_LCD_CTL_ACCESS_STATE_READ: 
                        accessState <= GRAPH_LCD_CTL_ACCESS_STATE_NONE;
                    GRAPH_LCD_CTL_ACCESS_STATE_WRITE1: 
                        accessState <= GRAPH_LCD_CTL_ACCESS_STATE_WRITE2;
                    GRAPH_LCD_CTL_ACCESS_STATE_WRITE2: 
                        accessState <= GRAPH_LCD_CTL_ACCESS_STATE_WRITE3;
                    GRAPH_LCD_CTL_ACCESS_STATE_WRITE3: 
                        accessState <= GRAPH_LCD_CTL_ACCESS_STATE_NONE;
                    default: 
                        accessState <= GRAPH_LCD_CTL_ACCESS_STATE_NONE;
                endcase
            end
        end
    end
    
    /* Configuration Addresses for the Address Datapaths */
    localparam GRAPH_LCD_CTL_ACCESS_CFG_DRIVE1      = 3'd0;    /* Drive A1 (random access address) */
    localparam GRAPH_LCD_CTL_ACCESS_CFG_INC0        = 3'd1;    /* Increment A0 and drive it out */
    localparam GRAPH_LCD_CTL_ACCESS_CFG_LOAD0       = 3'd2;    /* Load A0 from D0.  Drive the A1 random access address*/
    localparam GRAPH_LCD_CTL_ACCESS_CFG_LOAD1       = 3'd3;    /* Load A1 from FIFO F1 */
    localparam GRAPH_LCD_CTL_ACCESS_CFG_ENDOFLINE   = 3'd4;    /* Special increment at the end of a line */
    localparam GRAPH_LCD_CTL_ACCESS_CFG_DRIVE0      = 3'd5;    /* Drive the A0 during screen refreshing */
    reg [2:0] accessCfg;
    
    /* Address Datapath Config Address generation */
    always @(posedge op_clk) 
    begin
        if(reset)
        begin
            accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_DRIVE1;
        end
        else
        begin
            if(de)  /* screen refreshing is going on */
            begin
                if(vCount) /* the end of the line */
                begin
                    /* special addition at the end of the line */
                    accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_ENDOFLINE;
                end
                else    /* increment the buffer pointer along the line */
                begin
                    if(dotclk_reg == TransitionDotclkFalling)
                    begin
                        accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_INC0;
                    end
                    else
                    begin
                        accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_DRIVE0;
                    end
                end
            end
            else 
            begin
                case(accessState)
                    GRAPH_LCD_CTL_ACCESS_STATE_NONE:
                        if(~acc)
                        begin
                            accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_DRIVE0;
                        end
                        else
                        begin
                            if(~cmdFifoEmpty) 
                            begin
                                if(dotclk_reg == TransitionDotclkFalling)
                                begin
                                    accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_LOAD1;
                                end
                                else
                                begin
                                    accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_DRIVE1;
                                end
                            end
                            else
                            begin
                                /* reset the frame index at the begining of the screen */
                                if(vsyncInternal)
                                begin
                                    accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_LOAD0;
                                end
                                else
                                begin
                                    accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_DRIVE1;
                                end
                            end
                        end
                    GRAPH_LCD_CTL_ACCESS_STATE_LOAD:
                        /* During VSYNC interval access to the SRAM can be initiated continuosly. So
                        * a possibe is the case when access state machine doesn't progress through 
                        * NONE state. To guaranty the index reset, it is also performed in DECODE 
                        * state during VSYNC period. 
                        */
                        if(vsyncInternal)
                        begin
                            accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_LOAD0;
                        end
                        else
                        begin
                            accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_DRIVE1;
                        end     
                    default:
                        accessCfg <= GRAPH_LCD_CTL_ACCESS_CFG_DRIVE1;
                endcase
            end    
        end
    end    
      
    /* Generate the 3 control signals to control the operation of the external SRAM */
    
    /* Pulse the write signal for one cycle (active low) */
    always @(posedge op_clk)
    begin
        nwe <= ~(accessState == GRAPH_LCD_CTL_ACCESS_STATE_WRITE1);
    end
    
    /* Only drive the data from PSoC for 2 cycles during a write cycle (active high) */
    always @(posedge op_clk)
    begin
        doe <= (accessState == GRAPH_LCD_CTL_ACCESS_STATE_WRITE1) || 
        (accessState == GRAPH_LCD_CTL_ACCESS_STATE_WRITE2);
    end

    /* Disable driving from the SRAM for 4 cycles centered around the 2 cycles that */
    /* PSoC will be driving to avoid having both drive at the same time. (active low) */
    always @(posedge op_clk)
    begin
        noe <= ((accessState == GRAPH_LCD_CTL_ACCESS_STATE_DECODE) & ~cmdRead) || 
                (accessState == GRAPH_LCD_CTL_ACCESS_STATE_WRITE1)             ||
                (accessState == GRAPH_LCD_CTL_ACCESS_STATE_WRITE2)             ||
                (accessState == GRAPH_LCD_CTL_ACCESS_STATE_WRITE3);        
    end
        
    /* Latch the read data from the external RAM during the last cycle of the Read */    
    /* Data is latched from Parallel in to F1. F1 write mode configured to be edge */    
    reg getReadData;
    always @(posedge op_clk)
    begin
        getReadData <= (accessState == GRAPH_LCD_CTL_ACCESS_STATE_READ);       
    end
 
    /* The data DPs only have two configurations controlled by getWriteData
    *   0 - Drive the A0 value onto parallel out
    *   1 - Drive the A0 value onto parallel out and load a new A0 value from F0 */
    reg getWriteData;
    always @(posedge op_clk)
    begin
        getWriteData <= (accessState == GRAPH_LCD_CTL_ACCESS_STATE_DECODE) & ~cmdRead & dotclk_reg;
    end
    
    /***************************************************************************
    *                Interrupt generation 
    ***************************************************************************/   
    /* A selectable interrupt pulse is generated at the entry and exit of the
    *  horizontal and vertical blanking interval. Interrupt generation is 
    *  implemented as edge detecting for corresponding blanking interval. */

    wire int_src = (InterruptGen == INT_ON_VERT_BLANKING) ? v_blanking : 
                   (InterruptGen == INT_ON_VERT_AND_HORIZ_BLANKING) ? acc : 1'b0;
    generate
        if(InterruptGen == INT_NONE)
        begin: NoInterrupt
            assign interrupt = int_src;
        end
        else 
        begin: BlankInterrupt
            reg int_src_dly;
            always @(posedge op_clk)
            begin
                if(reset)
                begin
                    int_src_dly <= 1'b0;
                end
                else
                begin
                    int_src_dly <= int_src;
                end
            end
            assign interrupt = int_src ^ int_src_dly;
        end
    endgenerate
    
    /***************************************************************************  
    *   Address DataPath configuration memory
    *    A0 - Used to increment through the frame.  Loaded from D0 and then 
    *         incremented through the frame.
    *    A1 - Used for random accesses.  Loaded from the F1.
    *    D0 - Starting address for the entire frame of data.
    *    D1 - Increment at the end of a horizontal line.  In order to align the 
    *         addresses to a power of two boundary at the beginning of each line
    *         an increment other than 1 will be needed if the width of the screen 
    *         is not a power of 2.  This is done to make the address calculation
    *         easier.
    *    F0 - Not used.
    *    F1 - Used to provide address and command information.  The upper bit 
    *         determines whether the command is a read(1) or a write(0). Writing
    *         to the upper address byte indicates that the full command is 
    *         present. The other bytes (2 address bytes and 2 data bytes 
    *         (for reads)) must have already been written.
    *
    *    Configuration for AddrDp2 and AddrDp1 are similar. AddrDp0 uses different 
    *    configuration because of carry chain.
    *
    ***************************************************************************/
    
    localparam ADDRESS_DP_CONFIG = {
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0: DRIVE1 */
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1: INC0 */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2: LOAD0 */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3: LOAD1 */
        `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_D1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4: ENDOFLINE */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7: IDLE*/
        8'hFF, 8'h00,    /*CFG9:  */
        8'hFF, 8'hFF,    /*CFG11-10:  */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_CHAIN, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12:  */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
        1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14: */
        3'h00, `SC_FIFO_SYNC_NONE, 6'h00,    
        `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,   
        `SC_FIFO_LEVEL,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*CFG17-16: */
    };
    
    /* Signals tieing the 24 bit datapath together */
    wire [14:0] chain0;
    wire [14:0] chain1;
    
    /* Address DataPath 0 */
    cy_psoc3_dp #(.cy_dpconfig(
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0: DRIVE1 */
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1: INC0 */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2: LOAD0 */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3: LOAD1 */
        `CS_ALU_OP__ADD, `CS_SRCA_A0, `CS_SRCB_D1,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4: ENDOFLINE */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7: IDLE*/
        8'hFF, 8'h00,    /*CFG9:  */
        8'hFF, 8'hFF,    /*CFG11-10:  */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
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
    })) AddrDp0
    (
        /* input              */  .clk(op_clk),             
        /* input [02:00]      */  .cs_addr(accessCfg), 
        /* input              */  .route_si(1'b0),     
        /* input              */  .route_ci(1'b0),     
        /* input              */  .f0_load(1'b0),     
        /* input              */  .f1_load(1'b0),     
        /* input              */  .d0_load(1'b0),     
        /* input              */  .d1_load(1'b0),     
        /* output             */  .ce0(),             
        /* output             */  .cl0(),             
        /* output             */  .z0(),             
        /* output             */  .ff0(),             
        /* output             */  .ce1(),             
        /* output             */  .cl1(),             
        /* output             */  .z1(),             
        /* output             */  .ff1(),             
        /* output             */  .ov_msb(),         
        /* output             */  .co_msb(),         
        /* output             */  .cmsb(),             
        /* output             */  .so(),             
        /* output             */  .f0_bus_stat(),      
        /* output             */  .f0_blk_stat(),      
        /* output             */  .f1_bus_stat(),      
        /* output             */  .f1_blk_stat(),      
        /* input              */  .ci(1'b0),         
        /* output             */  .co(chain0[12]), 
        /* input              */  .sir(1'b0),     
        /* output             */  .sor(),         
        /* input              */  .sil(chain0[10]), 
        /* output             */  .sol(chain0[11]),
        /* input              */  .msbi(chain0[9]), 
        /* output             */  .msbo(),             
        /* input [01:00]      */  .cei(2'b0),     
        /* output [01:00]     */  .ceo(chain0[1:0]), 
        /* input [01:00]      */  .cli(2'b0), 
        /* output [01:00]     */  .clo(chain0[3:2]),
        /* input [01:00]      */  .zi(2'b0),         
        /* output [01:00]     */  .zo(chain0[5:4]),     
        /* input [01:00]      */  .fi(2'b0),         
        /* output [01:00]     */  .fo(chain0[7:6]), 
        /* input [01:00]      */  .capi(2'b0),    
        /* output [01:00]     */  .capo(chain0[14:13]),
        /* input              */  .cfbi(1'b0),             
        /* output             */  .cfbo(chain0[8]),         
        /* input [07:00]      */  .pi(),         
        /* output [07:00]     */  .po(addr0)     
    );

    /* Address DataPath 1 */
    cy_psoc3_dp #(.cy_dpconfig(ADDRESS_DP_CONFIG)) AddrDp1
    (
        /* input              */  .clk(op_clk),             
        /* input [02:00]      */  .cs_addr(accessCfg), 
        /* input              */  .route_si(1'b0),         
        /* input              */  .route_ci(1'b0),         
        /* input              */  .f0_load(1'b0),         
        /* input              */  .f1_load(1'b0),         
        /* input              */  .d0_load(1'b0),         
        /* input              */  .d1_load(1'b0),         
        /* output             */  .ce0(),             
        /* output             */  .cl0(),             
        /* output             */  .z0(),             
        /* output             */  .ff0(),             
        /* output             */  .ce1(),             
        /* output             */  .cl1(),             
        /* output             */  .z1(),             
        /* output             */  .ff1(),             
        /* output             */  .ov_msb(),         
        /* output             */  .co_msb(),         
        /* output             */  .cmsb(),             
        /* output             */  .so(),             
        /* output             */  .f0_bus_stat(),     
        /* output             */  .f0_blk_stat(),     
        /* output             */  .f1_bus_stat(),     
        /* output             */  .f1_blk_stat(),     
        /* input              */  .ci(chain0[12]),         
        /* output             */  .co(chain1[12]),         
        /* input              */  .sir(chain0[11]),     
        /* output             */  .sor(chain0[10]),     
        /* input              */  .sil(chain1[10]),     
        /* output             */  .sol(chain1[11]),     
        /* input              */  .msbi(chain1[9]),     
        /* output             */  .msbo(chain0[9]),     
        /* input [01:00]      */  .cei(chain0[1:0]),     
        /* output [01:00]     */  .ceo(chain1[1:0]),     
        /* input [01:00]      */  .cli(chain0[3:2]),     
        /* output [01:00]     */  .clo(chain1[3:2]),     
        /* input [01:00]      */  .zi(chain0[5:4]),     
        /* output [01:00]     */  .zo(chain1[5:4]),     
        /* input [01:00]      */  .fi(chain0[7:6]),     
        /* output [01:00]     */  .fo(chain1[7:6]),     
        /* input [01:00]      */  .capi(chain0[14:13]),    
        /* output [01:00]     */  .capo(chain1[14:13]),    
        /* input              */  .cfbi(chain0[8]),     
        /* output             */  .cfbo(chain1[8]),     
        /* input [07:00]      */  .pi(),         
        /* output [07:00]     */  .po(addr1)     
    );

    /* Address DataPath 2 */
    cy_psoc3_dp #(.cy_dpconfig(ADDRESS_DP_CONFIG)) AddrDp2
    (
        /* input              */  .clk(op_clk),         
        /* input [02:00]      */  .cs_addr(accessCfg),
        /* input              */  .route_si(1'b0),     
        /* input              */  .route_ci(1'b0),     
        /* input              */  .f0_load(1'b0),     
        /* input              */  .f1_load(1'b0),     
        /* input              */  .d0_load(1'b0),     
        /* input              */  .d1_load(1'b0),     
        /* output             */  .ce0(),             
        /* output             */  .cl0(),             
        /* output             */  .z0(),             
        /* output             */  .ff0(),         
        /* output             */  .ce1(),         
        /* output             */  .cl1(),         
        /* output             */  .z1(),             
        /* output             */  .ff1(),         
        /* output             */  .ov_msb(),         
        /* output             */  .co_msb(),         
        /* output             */  .cmsb(),             
        /* output             */  .so(),         
        /* output             */  .f0_bus_stat(),     
        /* output             */  .f0_blk_stat(),     
        /* output             */  .f1_bus_stat(notFullAddr2), 
        /* output             */  .f1_blk_stat(cmdFifoEmpty), 
        /* input              */  .ci(chain1[12]),             
        /* output             */  .co(),             
        /* input              */  .sir(chain1[11]),             
        /* output             */  .sor(chain1[10]),             
        /* input              */  .sil(1'b0),             
        /* output             */  .sol(),         
        /* input              */  .msbi(1'b0),             
        /* output             */  .msbo(chain1[9]),             
        /* input [01:00]      */  .cei(chain1[1:0]),     
        /* output [01:00]     */  .ceo(),     
        /* input [01:00]      */  .cli(chain1[3:2]),     
        /* output [01:00]     */  .clo(),      
        /* input [01:00]      */  .zi(chain1[5:4]),         
        /* output [01:00]     */  .zo(), 
        /* input [01:00]      */  .fi(chain1[7:6]),     
        /* output [01:00]     */  .fo(),     
        /* input [01:00]      */  .capi(chain1[14:13]),    
        /* output [01:00]     */  .capo(),    
        /* input              */  .cfbi(chain1[8]),             
        /* output             */  .cfbo(),     
        /* input [07:00]      */  .pi(),         
        /* output [07:00]     */  .po(addr2FromDP)
    );
    
    /* Two 8 bit DP elements are used for the data in and out path.
    *  All these Datapaths do is provide a path from the data input pins
    *  to the FIFO1 and from FIFO0 to the data output pins.
    */
    wire [14:0] chain2;
    
    /* Data DataPathes configuration */
    localparam DATA_DP_CONFIG = {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0: DRIVE */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1: LOAD */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6: IDLE*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7: IDLE*/
        8'hFF, 8'h00,    /*CFG9:  */
        8'hFF, 8'hFF,    /*CFG11-10:  */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12: */
        `SC_A0_SRC_PIN, `SC_SHIFT_SL, 1'h0,
        1'h0, `SC_FIFO1_ALU, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:  */
        10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*CFG17-16:  */
    };
    /* LSB DataPath */ 
    cy_psoc3_dp #(.cy_dpconfig(DATA_DP_CONFIG)) LsbDp
    (
        /* input             */  .clk(op_clk),             
        /* input [02:00]     */  .cs_addr({2'b0, getWriteData}), 
        /* input             */  .route_si(1'b0),         
        /* input             */  .route_ci(1'b0),         
        /* input             */  .f0_load(1'b0),        
        /* input             */  .f1_load(getReadData), 
        /* input             */  .d0_load(1'b0),         
        /* input             */  .d1_load(1'b0),         
        /* output            */  .ce0(),             
        /* output            */  .cl0(),             
        /* output            */  .z0(),             
        /* output            */  .ff0(),             
        /* output            */  .ce1(),             
        /* output            */  .cl1(),             
        /* output            */  .z1(),             
        /* output            */  .ff1(),             
        /* output            */  .ov_msb(),         
        /* output            */  .co_msb(),         
        /* output            */  .cmsb(),             
        /* output            */  .so(),             
        /* output            */  .f0_bus_stat(),     
        /* output            */  .f0_blk_stat(),     
        /* output            */  .f1_bus_stat(),     
        /* output            */  .f1_blk_stat(),     
        /* input             */  .ci(1'b0),         
        /* output            */  .co(chain2[12]), 
        /* input             */  .sir(1'b0),     
        /* output            */  .sor(),         
        /* input             */  .sil(chain2[10]), 
        /* output            */  .sol(chain2[11]),
        /* input             */  .msbi(chain2[9]), 
        /* output            */  .msbo(),             
        /* input [01:00]     */  .cei(2'b0),     
        /* output [01:00]    */  .ceo(chain2[1:0]), 
        /* input [01:00]     */  .cli(2'b0), 
        /* output [01:00]    */  .clo(chain2[3:2]),
        /* input [01:00]     */  .zi(2'b0),         
        /* output [01:00]    */  .zo(chain2[5:4]),     
        /* input [01:00]     */  .fi(2'b0),         
        /* output [01:00]    */  .fo(chain2[7:6]), 
        /* input [01:00]     */  .capi(2'b0),    
        /* output [01:00]    */  .capo(chain2[14:13]),
        /* input             */  .cfbi(1'b0),             
        /* output            */  .cfbo(chain2[8]),          
        /* input [07:00]     */  .pi(di_lsb),    
        /* output [07:00]    */  .po(do_lsb)     
    );

    /* MSB DataPath */
    cy_psoc3_dp #(.cy_dpconfig(DATA_DP_CONFIG)) MsbDp
    (
        /* input             */  .clk(op_clk),             
        /* input [02:00]     */  .cs_addr({2'b0, getWriteData}), 
        /* input             */  .route_si(1'b0),     
        /* input             */  .route_ci(1'b0),     
        /* input             */  .f0_load(1'b0),        
        /* input             */  .f1_load(getReadData),
        /* input             */  .d0_load(1'b0),     
        /* input             */  .d1_load(1'b0),     
        /* output            */  .ce0(),     
        /* output            */  .cl0(),     
        /* output            */  .z0(),         
        /* output            */  .ff0(),     
        /* output            */  .ce1(),     
        /* output            */  .cl1(),     
        /* output            */  .z1(),         
        /* output            */  .ff1(),     
        /* output            */  .ov_msb(),     
        /* output            */  .co_msb(),     
        /* output            */  .cmsb(),     
        /* output            */  .so(),         
        /* output            */  .f0_bus_stat(notFullData1),     
        /* output            */  .f0_blk_stat(),     
        /* output            */  .f1_bus_stat(avail),
        /* output            */  .f1_blk_stat(),     
        /* input             */  .ci(chain2[12]),             
        /* output            */  .co(),             
        /* input             */  .sir(chain2[11]),             
        /* output            */  .sor(chain2[10]),             
        /* input             */  .sil(1'b0),             
        /* output            */  .sol(),         
        /* input             */  .msbi(1'b0),             
        /* output            */  .msbo(chain2[9]),             
        /* input [01:00]     */  .cei(chain2[1:0]),     
        /* output [01:00]    */  .ceo(),     
        /* input [01:00]     */  .cli(chain2[3:2]),     
        /* output [01:00]    */  .clo(),      
        /* input [01:00]     */  .zi(chain2[5:4]),         
        /* output [01:00]    */  .zo(), 
        /* input [01:00]     */  .fi(chain2[7:6]),     
        /* output [01:00]    */  .fo(),     
        /* input [01:00]     */  .capi(chain2[14:13]),    
        /* output [01:00]    */  .capo(),    
        /* input             */  .cfbi(chain2[8]),             
        /* output            */  .cfbo(),     
        /* input [07:00]     */  .pi(di_msb),
        /* output [07:00]    */  .po(do_msb) 
    );

endmodule

`endif /* GraphicLCDCtrl_v1_60_V_ALREADY_INCLUDED */


