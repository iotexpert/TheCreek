/*******************************************************************************
*
* FILENAME:         bVoltageFaultDetector.v
* Component Name:   bVoltageFaultDetector
*
* DESCRIPTION:
*  The Voltage Fault Detector component provides a simple way to monitor
*  up to 32 voltage inputs against user-defined over and under voltage limits
*  without using the ADC and without having to write any firmware. The
*  component simply outputs a good/bad status result (“power good” or
*  pg[x]) for each voltage being monitored.
* 
*******************************************************************************
*                 Datapath Register Definitions
*******************************************************************************
*
*  INSTANCE NAME:  VFDp 
*
*  DESCRIPTION:
*    This data path counts cycles of over voltage/under voltage event to see
*    if it exeedes allowed range prior setting the fault status.
*
*  REGISTER USAGE:
*    F0 => not used
*    F1 => not used
*    D0 => Glitch filter length
*    D1 => Glitch filter length
*    A0 => Actual glitch filter value of overvoltage event for specific voltage 
*          input
*    A1 => Actual glitch filter value of overvoltage event for specific voltage 
*          input
*
*  DATA PATH STATES:
*    0 0 0   0   NOT USED:
*    0 0 1   1   NOT USED:
*    0 1 0   2   NOT USED:
*    0 1 1   3   NOT USED:
*    1 0 0   4   RESET: Reset bot OV and UV glitch filter counters for specific
*                       inputs to zero.
*    1 0 1   5   INC OV: Increments Overvoltage glithc filter counter
*    1 1 0   6   INC OV: Increments Undervoltage glithc filter counter
*    1 1 1   7   RESET: Reset bot OV and UV glitch filter counters for specific
*                       inputs to zero.
*
********************************************************************************
*                 I*O Signals:
********************************************************************************
*    Name              Direction       Description
*    pg_ov             input           Pgood signal from over voltage comparator
*    pg_uv             input           Pgood signal from under voltage 
*                                      comparator
*    en                input           Global enable signal
*    nrq               input           Feedback signal from DMA
*    clock             input           Clock signal used to drive all digital 
*                                      output signals
*    bus_clock         input           Bus Clock
*    actual_pg         input           It is a pgood signal of the previous scan
*                                      of a specific voltage
*    vs[4:0]           output          Addres lines used to select the voltage
*                                      input
*    pg_out[X:0]       output          Active high signal indicating v[x] is 
*                                      within range
*    vdac_wr           output          A pulse that triggers DMA to write new 
*                                      values to VDAC(s)
*    res_rd            output          A pulse that triggers DMA to read  
*                                      Glithch filter for currenly active 
*                                      voltage input
*    eoc               output          End of conversion signal
*
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
*******************************************************************************/

`include "cypress.v"
`ifdef bVoltageFaultDetector_v1_0_V_ALREADY_INCLUDED
`else
`define bVoltageFaultDetector_v1_0_V_ALREADY_INCLUDED

module bVoltageFaultDetector_v1_0(pg_ov,pg_uv,en,clock,bus_clock,vs,pg_out,
                                  pgood,vdac_wr,res_rd,eoc,nrq,actual_pg);
    
    parameter NumVoltages = 1;
    parameter CompareType = 0;
    
    localparam OV_UV   = 0;
    localparam OV_ONLY = 1;
    localparam UV_ONLY = 2;
    
    input wire pg_ov;
    input wire pg_uv;
    input wire en;
    input wire nrq;
    input wire clock;
    input wire bus_clock;
    input wire actual_pg;
    output wire pgood;
    output wire[4:0] vs;
    output wire [NumVoltages - 1 : 0] pg_out;
    output reg vdac_wr;
    output reg res_rd;
    output wire eoc;
    
    wire clk_fin;
    reg [NumVoltages - 1 : 0] pg_reg;
    reg [NumVoltages - 1 : 0] ov_reg;
    wire [7:0] control;
    wire [7:0] pg_status8;
    wire [7:0] pg_status16;
    wire [7:0] pg_status24;
    wire [7:0] pg_status32;
    
    wire [7:0] ov_status8;
    wire [7:0] ov_status16;
    wire [7:0] ov_status24;
    wire [7:0] ov_status32;
    
    wire [6:0]cycle_count;
    wire cycle_cnt_tc;
    wire [6:0] vs_count;
    wire vs_cnt_tc;
    
    reg cycle_cnt_tc_reg;
    reg pg_ov_latch;
    reg pg_uv_latch;
    wire sw_enable;
    wire en_fin = en & sw_enable;
    wire gf_ov_fault;
    wire gf_uv_fault;
    
    wire pg_ov_latch_wire;
    wire pg_uv_latch_wire;
    wire pgood_cond_wire;
    wire undervoltage_wire;
    wire overvoltage_wire;
    reg glitch_filter_enable;
    reg comp_clk_reg;
    reg bus_clk_reg;
    wire comb_logic;
    wire bus_clk_fin;
    reg overvoltage;
    reg undervoltage;
       
    wire check_cond               = (cycle_count[4:0] == 5'h02);
    wire end_of_cycle             = (cycle_count[4:0] == 5'h01);
    wire trig_vdac_dma            = (cycle_count[4:0] == 5'h0d);
    wire glitch_filter_capture    = (cycle_count[4:0] == 5'h04);
    wire read_write_glitch_filter = (cycle_count[4:0] == 5'h03);
    genvar i;
    reg current_pg;
    reg current_ov;
    reg is_start;
    wire cnt_en = is_start & en_fin;
    /*************************************************************************
    * UDB revisions
    **************************************************************************/
    localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);
    localparam CY_UDB_V1 = (!CY_UDB_V0);
       
    localparam CyclePeriod = 4'h0f;
    localparam VSPeriod    = NumVoltages - 1;
    
    localparam CTRL_SW_ENABLE = 1'b0; 
    
    /* Datapath configuration */
    localparam vfd_dp8_cfg = {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:  NOT USED*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:  NOT USED  */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:  NOT USED*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:   NOT USED  */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:  RESET*/
        `CS_ALU_OP__INC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:  INC OV*/
        `CS_ALU_OP__INC, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:  INC UV*/
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:  RESET*/
        8'hFF, 8'hFF,    /*CFG9:                */
        8'hFF, 8'hFF,    /*CFG11-10:                */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12:                */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'h0,
        1'h0, `SC_FIFO1__A1, `SC_FIFO0__A0,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:                */
        3'h00, `SC_FIFO_SYNC_NONE, 6'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO__EDGE,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL  /*CFG17-16:                */
    };
    
    /* Clock Enable primitive instantiation */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    ClkEn (
        .clock_in(clock),
        .enable(1'b1),
        .clock_out(clk_fin)
    );

    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    ClkVSCntEn (
        .clock_in(clock),
        .enable(cycle_cnt_tc_reg),
        .clock_out(clk_vs)
    );
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    BusClkEn (
        .clock_in(bus_clock),
        .enable(1'b1),
        .clock_out(bus_clk_fin)
    );
    
    assign vs[4:0]    = vs_count[4:0];
    assign comb_logic = (~comp_clk_reg & bus_clk_reg) | nrq;
    
    always@(posedge bus_clk_fin)
    begin
        if(en_fin)
        begin
            if(check_cond)
            begin
                is_start<= 1'b1;
            end
            else
            begin
                is_start <= is_start;
            end
        end
        else
        begin
            is_start <= 1'b0;
        end
    end
    
    always@(posedge bus_clk_fin)
    begin
        bus_clk_reg <= comb_logic;
    end
    
    always@(posedge clk_fin)
    begin
        comp_clk_reg     <= comb_logic;
        cycle_cnt_tc_reg <= cycle_cnt_tc;
    end
    
    assign eoc = (is_start) && comp_clk_reg;
    
    assign pgood = undervoltage;
    
    always@(posedge clk_fin)
    begin
        if(en_fin)
        begin
            if(CompareType == OV_UV)
            begin
                if(trig_vdac_dma)
                begin
                    if(is_start)
                    begin
                        vdac_wr <= 1'b1;
                    end
                    else
					begin
						vdac_wr <= 1'b0;
                    end
                    glitch_filter_enable <= 1'b0;
                end
                else if(glitch_filter_capture)
                begin
                    res_rd               <= 1'b0;
                    vdac_wr              <= 1'b0;
                    pg_ov_latch          <= pg_ov; 
                    pg_uv_latch          <= pg_uv;
                    glitch_filter_enable <= 1'b1;
                end
                else if(read_write_glitch_filter)
                begin
                    res_rd               <= 1'b1;              
                    vdac_wr              <= 1'b0;
                    overvoltage          <= gf_ov_fault;
                    undervoltage         <= gf_uv_fault;
                    glitch_filter_enable <= 1'b0;
                end
                else
                if(check_cond)
                begin
                    if(actual_pg)
                    begin
                        if(pg_ov_latch && pg_uv_latch)
                        begin
                            current_pg <= 1'b1;
                            current_ov <= 1'b1;
                        end
                        else
                        if(pg_ov_latch)
                        begin
                           if(undervoltage)
                           begin
                                current_pg <= 1'b0;
                                current_ov <= 1'b1;
                           end
                           else
                           begin
                                current_pg <= 1'b1;
                                current_ov <= 1'b1;
                           end
                        end
                        else
                        if(overvoltage)
                        begin
                            current_pg <= 1'b0;
                            current_ov <= 1'b0;
                        end
                        else
                        begin
                            current_pg <= 1'b1;
                            current_ov <= 1'b1;
                        end
                    end
                    else
                    begin
                        if(pg_ov_latch && pg_uv_latch)
                        begin
                            current_pg <= 1'b1;
                            current_ov <= 1'b1;
                        end
                        else
                        if(pg_ov_latch)
                        begin
                            current_pg <= 1'b0;
                            current_ov <= 1'b1;
                        end
                        else
                        begin
                            current_pg <= 1'b0;
                            current_ov <= 1'b0;
                        end
                    end               
                end
                else
                begin
                    res_rd               <= 1'b0;
                    vdac_wr              <= 1'b0;
                    glitch_filter_enable <= 1'b0;
                end
            end
            else if(CompareType == OV_ONLY)
              begin
                undervoltage         <= 1'b0;
                pg_uv_latch          <= 1'b1;
                if(trig_vdac_dma)
                begin
                     if(is_start)
                    begin
                        vdac_wr <= 1'b1;
                    end
                    else
					begin
						vdac_wr <= 1'b0;
                    end
                    glitch_filter_enable <= 1'b0;
                    overvoltage          <= 1'b0;
                end
                else if(glitch_filter_capture)
                begin
                    res_rd               <= 1'b0;
                    vdac_wr              <= 1'b0;
                    pg_ov_latch          <= pg_ov; 
                    overvoltage          <= 1'b0;
                    glitch_filter_enable <= 1'b1;
                end
                else if(read_write_glitch_filter)
                begin
                    
                    res_rd               <= 1'b1;              
                    vdac_wr              <= 1'b0;
                    overvoltage          <= gf_ov_fault;
                    glitch_filter_enable <= 1'b0;
                end
                else
                if(check_cond)
                begin
                    if(actual_pg)
                    begin
                        if(pg_ov_latch)
                        begin
                            current_pg <= 1'b1;
                            current_ov <= 1'b1;
                        end
                        else
                        if(overvoltage)
                        begin
                            current_pg <= 1'b0;
                            current_ov <= 1'b0;
                        end
                        else
                        begin
                            current_pg <= 1'b1;
                            current_ov <= 1'b1;
                        end
                    end
                    else
                    begin
                        if(pg_ov_latch)
                        begin
                            current_pg <= 1'b1;
                            current_ov <= 1'b1;
                        end
                        else
                        begin
                            current_pg <= 1'b0;
                            current_ov <= 1'b0;
                        end
                    end               
                end
                else
                begin
                    res_rd               <= 1'b0;
                    vdac_wr              <= 1'b0;
                    glitch_filter_enable <= 1'b0;
                end
            end
            else
             begin
                overvoltage          <= 1'b0;
                pg_ov_latch          <= 1'b1;
                if(trig_vdac_dma)
                begin
                    if(is_start)
                    begin
                        vdac_wr <= 1'b1;
                    end
                    else
					begin
						vdac_wr <= 1'b0;
                    end
                    glitch_filter_enable <= 1'b0;
                    undervoltage          <= 1'b0;
                end
                else if(glitch_filter_capture)
                begin
                    res_rd               <= 1'b0;
                    vdac_wr              <= 1'b0;
                    pg_uv_latch          <= pg_uv; 
                    undervoltage         <= 1'b0;
                    glitch_filter_enable <= 1'b1;
                end
                else if(read_write_glitch_filter)
                begin
                    
                    res_rd               <= 1'b1;              
                    vdac_wr              <= 1'b0;
                    undervoltage          <= gf_uv_fault;
                    glitch_filter_enable <= 1'b0;
                end
                else
                if(check_cond)
                begin
                    if(actual_pg)
                    begin
                        if(pg_uv_latch)
                        begin
                            current_pg <= 1'b1;
                        end
                        else
                        if(undervoltage)
                        begin
                            current_pg <= 1'b0;
                        end
                        else
                        begin
                            current_pg <= 1'b1;
                        end
                    end
                    else
                    begin
                        if(pg_uv_latch)
                        begin
                            current_pg <= 1'b1;
                        end
                        else
                        begin
                            current_pg <= 1'b0;
                        end
                    end               
                end
                else
                begin
                    res_rd               <= 1'b0;
                    vdac_wr              <= 1'b0;
                    glitch_filter_enable <= 1'b0;
                end
            end
        end
        else 
        begin
            vdac_wr <= 1'b0;
        end
    end
        

    generate
    for(i = 0; i < NumVoltages; i = i + 1)
    begin:PG_GEN
        if(CompareType == OV_UV)
        begin
            always@(posedge clk_fin)
            begin
                if(is_start && end_of_cycle && (vs_count[4:0] == i))
                begin
                    pg_reg[i] <= current_pg;
                    ov_reg[i] <= ~current_ov;
                end
                else
                begin
                    pg_reg[i] <= pg_reg[i];
                    ov_reg[i] <= ov_reg[i];
                end
            end
        end
        //else
        if(CompareType == OV_ONLY)
        begin
           always@(posedge clk_fin)
            begin
                if(end_of_cycle && (vs_count[4:0] == i))
                begin
                    pg_reg[i] <= current_pg;
                    ov_reg[i] <= ~current_ov;
                end
                else
                begin
                    pg_reg[i] <= pg_reg[i];
                    ov_reg[i] <= ov_reg[i];
                end
            end
        end
        //else
        if(CompareType == UV_ONLY)
        begin
            always@(posedge clk_fin)
            begin
                ov_reg[i] <= 1'b0;
                if(end_of_cycle && (vs_count[4:0] == i))
                begin
                    pg_reg[i] <= current_pg;
                end
                else
                begin
                    pg_reg[i] <= pg_reg[i];
                end
            end
        end
     
        assign pg_out[i] = pg_reg[i];
                
        if(i <= 7)
        begin
            assign pg_status8[i] = pg_reg[i];
            assign ov_status8[i] = ov_reg[i];
        end
        else
        if(i <= 15)
        begin
            assign pg_status16[i-8] = pg_reg[i];
            assign ov_status16[i-8] = ov_reg[i];
        end
        else
        if(i <= 23)
        begin
            assign pg_status24[i-16] = pg_reg[i];
            assign ov_status24[i-16] = ov_reg[i];

        end
        else
        begin
            assign pg_status32[i-24] = pg_reg[i];
            assign ov_status32[i-24] = ov_reg[i];
        end
    end
    endgenerate
    
    /* Datapath instantiation */
    cy_psoc3_dp8 #(.cy_dpconfig_a(vfd_dp8_cfg))
    VFDp(
        /*  input           */ .clk(clk_fin),
        /*  input           */ .reset(1'b0),
        /*  input   [02:00] */ .cs_addr({glitch_filter_enable, pg_ov_latch, pg_uv_latch}),
        /*  input           */ .route_si(1'b0),
        /*  input           */ .route_ci(1'b0),
        /*  input           */ .f0_load(1'b0),
        /*  input           */ .f1_load(1'b0),
        /*  input           */ .d0_load(1'b0),
        /*  input           */ .d1_load(1'b0),
        /*  output          */ .ce0(gf_ov_fault),
        /*  output          */ .cl0(),
        /*  output          */ .z0(),
        /*  output          */ .ff0(),
        /*  output          */ .ce1(gf_uv_fault),
        /*  output          */ .cl1(),
        /*  output          */ .z1(),
        /*  output          */ .ff1(),
        /*  output          */ .ov_msb(),
        /*  output          */ .co_msb(),
        /*  output          */ .cmsb(),
        /*  output          */ .so(),
        /*  output          */ .f0_bus_stat(),
        /*  output          */ .f0_blk_stat(),
        /*  output          */ .f1_bus_stat(),
        /*  output          */ .f1_blk_stat()
    );
    
    if(CY_UDB_V0)
    begin: AsyncCtl
        cy_psoc3_control #(.cy_force_order(1))
        CtrlReg(
            /* output [07:00] */  .control(control)
        );
    end
    else
    begin: SyncCtl
        cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h0), .cy_ctrl_mode_0(8'h01))
        CtrlReg(
            /*  input         */ .clock(clk_fin),
            /* output [07:00] */ .control(control)
        );
    end
    
    assign sw_enable = control[CTRL_SW_ENABLE];
    
    cy_psoc3_count7 #(.cy_period(CyclePeriod),.cy_route_ld(0),.cy_route_en(1))
	CycleCounter(
        /* input          */ .clock(clk_fin),
        /* input          */ .reset(1'b0),
        /* input          */ .load(1'b0),
        /* input          */ .enable(en_fin),
        /* output [06:00] */ .count(cycle_count),
        /* output         */ .tc(cycle_cnt_tc)
    );
    
    cy_psoc3_count7 #(.cy_period(VSPeriod),.cy_route_ld(0),.cy_route_en(1))
    VSCounter(
        /* input          */ .clock(clk_vs),
        /* input          */ .reset(1'b0),
        /* input          */ .load(1'b0),
        /* input          */ .enable(en_fin),
        /* output [06:00] */ .count(vs_count),
        /* output         */ .tc(vs_cnt_tc)
    );
    
    generate
    if(NumVoltages <= 8)
    begin:Sts8
        if(NumVoltages != 8)
        begin
            assign pg_status8[7:NumVoltages] = 0;
            if(CompareType != UV_ONLY)
            begin
                assign ov_status8[7:NumVoltages] = 0;         
            end
        end
        
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg8(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status8)
        );
        if(CompareType != UV_ONLY)
        begin:OV
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg8(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status8)
            );
        end
    end
    else
    if(NumVoltages <= 16)
    begin:Sts16
        
        if(NumVoltages != 16)
        begin
            assign pg_status16[7:NumVoltages-8] = 0;
            if(CompareType != UV_ONLY)
            begin
                 assign ov_status16[7:NumVoltages-8] = 0;         
            end
        end
        
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg8(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status8)
        );
        
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg16(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status16)
        );
        
        if(CompareType != UV_ONLY)
        begin:OV
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg8(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status8)
            );
            
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg16(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status16)
            );
        end
    end
    else
    if(NumVoltages <= 24)
    begin:Sts24   
        if(NumVoltages != 24)
        begin
            assign pg_status16[7:NumVoltages-16] = 0;
                       
            if(CompareType != UV_ONLY)
            begin
                assign ov_status16[7:NumVoltages-16] = 0;
            end
        end
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg8(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status8)
        );
        
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg16(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status16)
        );
        
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg24(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status24)
        );     
        
        if(CompareType != UV_ONLY)
        begin:OV
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg8(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status8)
            );
            
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg16(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status16)
            );
            
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg24(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status24)
            );
        end
    end
    else
    if(NumVoltages <= 32)
    begin:Sts32
        
        if(NumVoltages != 32)
        begin
            assign pg_status32[7:NumVoltages-24] = 0;
                        
            if(CompareType != UV_ONLY)
            begin
                assign ov_status32[7:NumVoltages-24] = 0;
            end
        end
               
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg8(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status8)
        );
        
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg16(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status16)
        );
        
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg24(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status24)
        );
        
        cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
        PGStsReg32(
            /* input            */ .clock(clk_fin),
            /* output   [07:00] */ .status(pg_status32)
        );
        
        if(CompareType != UV_ONLY)
        begin:OV
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg8(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status8)
            );
            
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg16(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status16)
            );
            
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg24(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status24)
            );
            
            cy_psoc3_status #(.cy_force_order(1), .cy_md_select(8'h0))
            OVStsReg32(
                /* input            */ .clock(clk_fin),
                /* output   [07:00] */ .status(ov_status32)
            );
        end
    end 
    endgenerate
endmodule

`endif /* bVoltageFaultDetector_v1_0_V_ALREADY_INCLUDED */
//[] END OF FILE
