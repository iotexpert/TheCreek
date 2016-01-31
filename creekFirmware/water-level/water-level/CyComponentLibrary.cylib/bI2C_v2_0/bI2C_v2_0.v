/******************************************************************************* 
* File Name: bI2C_v2_0.v 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*  
* Description: 
*  This file provides a base model of the I2C componnent defining and all of the 
*  necessary interconnect.
* 
* Note: 
*
********************************************************************************
*                 Control and Status Register definitions
******************************************************************************** 
*
* TBD
******************************************************************************** 
*               Data Path register definitions   
******************************************************************************** 
*  INSTANCE NAME:  DatapathName 
*  DESCRIPTION: 
*  REGISTER USAGE: 
*   F0 => na 
*   F1 => na 
*   D0 => na  
*   D1 => na 
*   A0 => na 
*   A1 => na  
*   TBD
******************************************************************************** 
*               I*O Signals:   
******************************************************************************** 
*  IO SIGNALS: 
*  TBD
* 
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef bI2C_v2_0_V_ALREADY_INCLUDED
`else
`define bI2C_v2_0_V_ALREADY_INCLUDED

module bI2C_v2_0
(
    input wire clock,
    input wire reset,
    input wire scl_in,
    input wire sda_in,
    output reg sda_out,
    output reg scl_out,
    output wire irq 
);

   /**************************************************************************/
   /* Parameters                                                             */
   /**************************************************************************/
    localparam I2C_MODE_SLAVE                = 3'h1;
    localparam I2C_MODE_MASTER               = 3'h2;
    localparam I2C_MODE_MULTI_MASTER         = 3'h6;
    localparam I2C_MODE_MULTI_MASTER_SLAVE   = 3'h7;
    
    parameter [2:0] Mode = I2C_MODE_SLAVE;
    
    localparam I2C_CTRL_UNUSED7         = 3'h7; 
    localparam I2C_CTRL_STOP_GEN        = 3'h6;     /* generate a stop */
    localparam I2C_CTRL_RESTART_GEN     = 3'h5;     /* generate a restart */
    localparam I2C_CTRL_NACK            = 3'h4;     /* 1 = NAK on read, 0 = another byte */
    localparam I2C_CTRL_ANY_ADDRESS     = 3'h3;     /* interrupt on any address */
    localparam I2C_CTRL_TRANSMIT        = 3'h2;     /* 1 = transmit, 0 = receive */
    localparam I2C_CTRL_EN_MASTER       = 3'h1;     /* enable master */
    localparam I2C_CTRL_EN_SLAVE        = 3'h0;     /* enable slave*/  

    
    localparam I2C_STS_LOST_ARB         = 3'h6;     /* This bit is set immediately on lost arbitration  => ????? */
    localparam I2C_STS_STOP             = 3'h5;     /* 1 = a Stop condition was detected => transparent */ 
    localparam I2C_STS_BUS_BUSY         = 3'h4;    
    localparam I2C_STS_ADDRESS          = 3'h3;     /* => transparent */ 
    localparam I2C_STS_UNUSED2          = 3'h2;    
    localparam I2C_STS_LRB              = 3'h1;     /* Last Received Bit  => transparent */
    localparam I2C_STS_BYTE_COMPLETE    = 3'h0;     /* appear each matched address and data byte => sticky*/
    
    
    /**************************************************************************/
    /* Common Signals for Master and Slave                                    */
    /**************************************************************************/
    wire sda_negedge_detect;
    wire sda_posedge_detect;
    wire scl_negedge_detect;
    wire scl_posedge_detect;
    
    wire ctrl_stop_gen;     
    wire ctrl_restart_gen;
    wire ctrl_nack;
    wire ctrl_any_address;
    wire ctrl_transmit;
    wire ctrl_en_master;
    wire ctrl_en_slave;
    
    wire load_dummy;
    wire shift_enable;
    wire tx_reg_empty;
    wire i2c_reset;
    wire i2c_enable;
    wire [2:0] cs_addr_shifter;
    wire [6:0] status;                        /* Status Register Input */
    wire [7:0] control;            
    
    /* Internal signal register type declaration*/
    reg sda_in_dly;
    reg scl_in_dly;
    reg[4:0] state;
    
    /**************************************************************************/
    /* Signals for Master                                                     */
    /**************************************************************************/
    //localparam I2C_IDLE         = 5'h0;

    localparam I2C_PRE_START    = 5'h2;
    localparam I2C_START        = 5'h3;
    localparam I2C_RESTART      = 5'h4;
    localparam I2C_STOP         = 5'h5;

    localparam I2C_TX_D0        = 5'h8;
    localparam I2C_TX_D1        = 5'h9;
    localparam I2C_TX_D2        = 5'ha;
    localparam I2C_TX_D3        = 5'hb;
    localparam I2C_TX_D4        = 5'hc;
    localparam I2C_TX_D5        = 5'hd;
    localparam I2C_TX_D6        = 5'he;
    localparam I2C_TX_D7        = 5'hf;

    localparam I2C_RX_D0        = 5'h10;
    localparam I2C_RX_D1        = 5'h11;
    localparam I2C_RX_D2        = 5'h12;
    localparam I2C_RX_D3        = 5'h13;
    localparam I2C_RX_D4        = 5'h14;
    localparam I2C_RX_D5        = 5'h15;
    localparam I2C_RX_D6        = 5'h16;
    localparam I2C_RX_D7        = 5'h17;

    localparam I2C_RX_STALL     = 5'h06;
    localparam I2C_TX_STALL     = 5'h07;

    localparam I2C_RX_ACK       = 5'h1e;
    localparam I2C_TX_ACK       = 5'h1f;
 
    
    wire lost_arb;
    wire start_detect; /* Not generated for simple Master */
    wire stop_detect; /* Not generated for simple Master */
    wire clkgen_tc;
    wire clkgen_ce1; 
    wire clkgen_cl1;
    wire txdata;
    wire rxdata;
    wire stalled;
    wire busy;
    wire contention;
    wire enable_clkgen;
    wire [2:0] cs_addr_clkgen;
    
    /* Internal signal register type declaration*/
    reg bus_busy;
    reg lrb;
    reg byte_complete;
    reg clkgen_tc1; 
    reg clkgen_tc2; 
    reg contention1;
    
    
    /**************************************************************************/
    /* Signals for Slave                                                      */
    /**************************************************************************/
    localparam I2C_IDLE      = 3'h0;
    localparam I2C_RX_DATA   = 3'h1;
    //localparam I2C_RX_STALL  = 3'h2;
    //localparam I2C_RX_ACK    = 3'h3;
    localparam I2C_TX_DATA   = 3'h4;
   // localparam I2C_TX_ACK    = 3'h5;
    //localparam I2C_TX_STALL  = 3'h6;

    localparam I2C_OWN_ADDR  = 7'h05;
    
    /* Device Family and Silicon Revision definitions */
                          
    /* PSoC3 ES2 or earlier */
    localparam PSOC3_ES2 = ((`CYDEV_CHIP_MEMBER_USED   == `CYDEV_CHIP_MEMBER_3A) && 
                            (`CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_3A_ES2));
    /* PSoC5 ES1 or earlier */                        
    localparam PSOC5_ES1 = ((`CYDEV_CHIP_MEMBER_USED   == `CYDEV_CHIP_MEMBER_5A) && 
                            (`CYDEV_CHIP_REVISION_USED <= `CYDEV_CHIP_REVISION_5A_ES1));
    
    wire counter_load;
    wire counter_enable;
    wire count_eq_zero;
    wire op_clk;
    wire [2:0] count;
    wire [3:0] foo;
    
    /* Internal signal register type declaration*/
    reg address;
    reg start_sample;
    
   
    /**************************************************************************/
    /* hierarchy - instantiating another module                               */
    /**************************************************************************/
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    ClkSync(
        /* input  */.clock_in(clock),
        /* input  */.enable(i2c_enable),
        /* output */.clock_out(op_clk)
    );
            
    /* Instantiate the control register */
    generate
        if (PSOC3_ES2 || PSOC5_ES1)
        begin: AsyncCtl
            cy_psoc3_control #(.cy_force_order(`TRUE)) 
            CtrlReg (
                /* output 07:00] */.control(control)
            );
        end
        else
        begin: SyncCtl
            /* The clock to operate Control Reg for ES3 must be synchronous and run
            *  continuosly. In this case the udb_clock_enable is used only for 
            *  synchronization. The resulted clock is always enabled.
            */ 
            cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
            CtlClkSync
            (
                /* input  */    .clock_in(clock),
                /* input  */    .enable(1'b1),
                /* output */    .clock_out(ctl_clk)
            );  
        
            cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFF))
            CtrlReg (
                /* input          */ .clock(ctl_clk),
                /* output [07:00] */ .control(control)
            );
        end
    endgenerate
    
       
    /* Instantiate the status register and interrupt hook */
    cy_psoc3_statusi #(.cy_force_order(`TRUE), .cy_md_select(7'h60), .cy_int_mask(7'h00)) 
    StsReg (
        /* input          */ .clock(op_clk),
        /* input  [06:00] */ .status(status),
        /* output         */ .interrupt(irq)
    );
    
    /* Shifter.                                                               
     * Datapath is shifting into and out of A0.                               
     * User writes TX data to A0 and reads TX data from A0, always stalled.   
     * F1 status is used to synchronize start of transmission.                
     */
    cy_psoc3_dp8 #(.d0_init_a(8'h04), .cy_dpconfig_a(
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP___SL, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:IDLE */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:SHIFT A0 */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP___SL, `CS_A0_SRC_NONE, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG2 Comment:LOAD A1 from F1 */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG3 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
          8'hFF, 8'h00,    /*SC_REG4    Comment: */
          8'hFF, 8'h7F,    /*SC_REG5    Comment:Address Mask */
        `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_ENBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_ROUTE, /*SC_REG6 Comment: */
        `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
        1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*SC_REG7 Comment: */
        3'h0, `SC_FIFO_SYNC__ADD, 6'h0,   
        `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    })) Shifter(
    /*  input                   */ .clk(op_clk),
    /*  input   [02:00]         */ .cs_addr(cs_addr_shifter),
    /*  input                   */ .route_si(sda_in),
    /*  input                   */ .route_ci(1'b0),
    /*  input                   */ .f0_load(1'b0),
    /*  input                   */ .f1_load(1'b0),
    /*  input                   */ .d0_load(1'b0),
    /*  input                   */ .d1_load(1'b0),
    /*  output                  */ .ce0(address_match),
    /*  output                  */ .cl0(),
    /*  output                  */ .z0(),
    /*  output                  */ .ff0(),
    /*  output                  */ .ce1(),
    /*  output                  */ .cl1(),
    /*  output                  */ .z1(),
    /*  output                  */ .ff1(),
    /*  output                  */ .ov_msb(),
    /*  output                  */ .co_msb(),
    /*  output                  */ .cmsb(),
    /*  output                  */ .so(shift_data_out),
    /*  output                  */ .f0_bus_stat(), 
    /*  output                  */ .f0_blk_stat(),
    /*  output                  */ .f1_bus_stat(), 
    /*  output                  */ .f1_blk_stat(tx_reg_empty)
    );
    
    generate
        if ((Mode == I2C_MODE_MASTER) || (Mode == I2C_MODE_MULTI_MASTER) || ( Mode == I2C_MODE_MULTI_MASTER_SLAVE))
        begin: Master
            /* Clock Generator.                                                       
             * Use only for Master, Multi=Master and Multi-Master-Slave.              
             * Datapath is counting samples at divide by 8.                           
             * State changes on "tc".                                                 
             * Output clock is "cmpl0" (compare < 4).                                 
             */
            cy_psoc3_dp8 #(.d0_init_a(8'd15), .d1_init_a(8'd8), .cy_dpconfig_a(
            {
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:IDLE */
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:DEC A0 */
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG2 Comment:IDLE */
                `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_A0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG3 Comment:LOAD A0 from D0 */
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG4 Comment: */
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG5 Comment: */
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG6 Comment: */
                `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                `CS_CMP_SEL_CFGA, /*CS_REG7 Comment: */
                  8'hFF, 8'h00,    /*SC_REG4    Comment: */
                  8'hFF, 8'h7F,    /*SC_REG5    Comment:Address Mask */
                `SC_CMPB_A0_D1, `SC_CMPA_A0_D1, `SC_CI_B_ARITH,
                `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_ENBL,
                `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
                `SC_SI_A_ROUTE, /*SC_REG6 Comment: */
                `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
                1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
                `SC_MSB_DSBL, `SC_MSB_BIT7, `SC_MSB_NOCHN,
                `SC_FB_NOCHN, `SC_CMP1_NOCHN,
                `SC_CMP0_NOCHN, /*SC_REG7 Comment: */
                3'h0, `SC_FIFO_SYNC__ADD, 6'h0,
                `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
                `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
                `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
            })) ClkGen(
            /*  input                   */  .clk(op_clk),
            /*  input   [02:00]         */  .cs_addr(cs_addr_clkgen),
            /*  input                   */  .route_si(1'b0),
            /*  input                   */  .route_ci(1'b0),
            /*  input                   */  .f0_load(1'b0),
            /*  input                   */  .f1_load(1'b0),
            /*  input                   */  .d0_load(1'b0),
            /*  input                   */  .d1_load(1'b0),
            /*  output                  */  .ce0(),
            /*  output                  */  .cl0(),
            /*  output                  */  .z0(clkgen_tc),
            /*  output                  */  .ff0(),
            /*  output                  */  .ce1(clkgen_ce1),
            /*  output                  */  .cl1(clkgen_cl1),
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
        if ((Mode == I2C_MODE_SLAVE) || ( Mode == I2C_MODE_MULTI_MASTER_SLAVE))
        begin: Slave
            /* Used for bit counting for address and data shift in and out. */
            cy_psoc3_count7 #(.cy_period(7'd7),.cy_route_ld(1),.cy_route_en(1)) BitCounter(
                /*  input             */ .clock(op_clk),
                /*  input             */ .reset(i2c_reset),
                /*  input             */ .load(counter_load),
                /*  input             */ .enable(counter_enable),
                /*  output  [06:00]   */ .count({foo,count}),
                /*  output            */ .tc()
            );
        end
    endgenerate
    
    
    /* Control Register Implementation */
    assign ctrl_stop_gen      = control[I2C_CTRL_STOP_GEN];
    assign ctrl_restart_gen   = control[I2C_CTRL_RESTART_GEN];
    assign ctrl_nack          = control[I2C_CTRL_NACK];
    assign ctrl_any_address   = control[I2C_CTRL_ANY_ADDRESS];
    assign ctrl_transmit      = control[I2C_CTRL_TRANSMIT];
    assign ctrl_en_master     = control[I2C_CTRL_EN_MASTER];
    assign ctrl_en_slave      = control[I2C_CTRL_EN_SLAVE];

    /* Status Register Implementation */
    /* TODO: ��������� ����� ��� I2C_STS_BYTE_COMPLETE 
             ������� ������� ������ I2C_MODE_MULTI_MASTER_SLAVE  */  
    assign status[I2C_STS_LOST_ARB]         = (Mode == I2C_MODE_MULTI_MASTER) ? lost_arb : 1'b0;  /* sticky      */
    assign status[I2C_STS_STOP]             = stop_detect;                                  /* sticky      */
    assign status[I2C_STS_BUS_BUSY]         = ((Mode == I2C_MODE_MASTER) || (Mode == I2C_MODE_MULTI_MASTER)) ? bus_busy : 1'b0;  /* transparent */
    assign status[I2C_STS_ADDRESS]          = ((Mode == I2C_MODE_MASTER) || (Mode == I2C_MODE_MULTI_MASTER)) ? 1'b0 : address;   /* transparent */
    assign status[I2C_STS_UNUSED2]          = 1'b0;
    assign status[I2C_STS_LRB]              = lrb;                                          /* transparent */
    assign status[I2C_STS_BYTE_COMPLETE]    = byte_complete;                                /* transparent for Master, sticky for Slave. Why ????*/
        

    /**************************************************************************/
    /* Start/Stop Detect (Multimaster and Slave only).                        */ 
    /* Not generated for simple Master                                        */
    /**************************************************************************/

    /* Need edge detect for start and stop */
    /* TODO: ������� ������� ������ reset*/
    always @(posedge op_clk)
    begin
        sda_in_dly <= sda_in;
    end

    always @(posedge op_clk)
    begin
        scl_in_dly <= scl_in;
    end

    assign sda_negedge_detect = ~sda_in & sda_in_dly;
    assign sda_posedge_detect = sda_in & ~sda_in_dly;
    assign scl_negedge_detect = ~scl_in & scl_in_dly;
    assign scl_posedge_detect = scl_in & ~scl_in_dly;

    /* Compute start and stop                                                  */
    /* For Slave the start detect resets all state in the block, this excepted */
    /* TODO: ������� ������� ������ I2C_MODE_MULTI_MASTER_SLAVE */
    assign start_detect = ((Mode == I2C_MODE_MULTI_MASTER) || (Mode == I2C_MODE_SLAVE)) ? 
                          (scl_in & sda_negedge_detect) : 1'b0;
    assign stop_detect  = ((Mode == I2C_MODE_MULTI_MASTER) || (Mode == I2C_MODE_SLAVE)) ? 
                          (scl_in & sda_posedge_detect) : 1'b0;
   
    
    /******************************************************************************************************************/

    
    /**************************************************************************
    * Bit Controller.                                                        
    **************************************************************************/
    /* SCL out */
    /* TODO: Support I2C_MODE_MULTI_MASTER_SLAVE must be added */
    generate
        if ((Mode == I2C_MODE_MASTER) || (Mode == I2C_MODE_MULTI_MASTER))
        begin
            /* Need to manually register TC until we can have both 
             * registered and combo version out
             */
             /* TODO: Sunc Reset must be used */
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset)   clkgen_tc1 <= 1'b1;
                else             clkgen_tc1 <= clkgen_tc;
            end

            /* Delayed so data can be changed one cycle after clock for hold */
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset)   clkgen_tc2 <= 1'b1;
                else             clkgen_tc2 <= clkgen_tc1;
            end

            /* SCL_OUT */
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset) 
                    scl_out <= 1'b1;
                else
                if (stalled)
                    scl_out <= 1'b0;
                else
                if (state == I2C_RESTART & clkgen_tc1)
                    scl_out <= 1'b1;
                else
                if (state == I2C_START & clkgen_tc1)
                    scl_out <= clkgen_cl1;
                else
                if (state == I2C_STOP & clkgen_tc1)
                    scl_out <= 1'b1;
                else
                if (state != I2C_IDLE & state != I2C_START & state != I2C_PRE_START) 
                    scl_out <= clkgen_cl1;
                else
                    scl_out <= 1'b1;
            end
            
            /* SDA_OUT */
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset) 
                    sda_out <= 1'b1;
                else
                begin
                    if (clkgen_tc2)     /* Maybe this is latch !!!!!!*/
                    begin
                        if ((state == I2C_START | state == I2C_STOP) | (state == I2C_RX_ACK & ~ctrl_nack)) 
                            sda_out <= 1'b0;
                        else
                        if (txdata) 
                            sda_out <= shift_data_out;
                        else
                            sda_out <= 1'b1;
                    end
                end
            end
        end
        else if (Mode == I2C_MODE_SLAVE)
        begin
            /* Compute SCL_OUT for stalling
             * once scl_out goes low it's "latched" because the master stops
             * released when we change state because of a FIFO write
             */   
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset) scl_out <= 1'b1;
                else
                begin
                    if (state == I2C_RX_STALL & scl_in == 1'b0)
                        scl_out <= 1'b0;
                    else
                    if (state == I2C_TX_STALL & scl_in == 1'b0)
                        scl_out <= 1'b0;
                    else
                        scl_out <= 1'b1;
                end
            end

            /* Compute SDA out
             * change only on the negative edge of SCL
             */
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset) sda_out <= 1'b1;
                else
                begin
                    /* Output data only on the negative edge or if the clock is low */
                    if (scl_negedge_detect | scl_out == 1'b0)
                    begin
                        if (state == I2C_TX_STALL | state == I2C_TX_DATA)    
                            sda_out <= shift_data_out;
                        else
                        if (state == I2C_RX_STALL | state == I2C_RX_ACK)     
                            sda_out <= 1'b0;
                        else                        
                            sda_out <= 1'b1;
                    end
                end
            end
        end
    endgenerate


    /**************************************************************************/
    /* Protocol State Machine.                                                */
    /* Includes states and byte complete computation.                         */
    /* State changes are aligned with input shifting which is on the positive */
    /* edge of SCL.                                                           */ 
    /* There are two flows: RX data/address, and TX data, and the address     */
    /* state bit distinguishes between RX data and address                    */
    /**************************************************************************/
    generate
        if ((Mode == I2C_MODE_MASTER) || (Mode == I2C_MODE_MULTI_MASTER))
        begin
        
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset) 
                begin
                      state <= I2C_IDLE;
                end
                else
                begin


                if (clkgen_tc1)
                begin

                if (lost_arb)    state <= I2C_IDLE;
                else
                case (state)

                I2C_IDLE: 
                begin
                    if (~tx_reg_empty & ~busy)     state <= I2C_PRE_START;
                    else                           state <= I2C_IDLE;
                end

                I2C_PRE_START:
                begin
                    state <= I2C_START;
                end

                I2C_START:
                begin
                    state <= I2C_TX_D0;
                end

                I2C_TX_D0:
                begin
                    state <= I2C_TX_D1;
                end

                I2C_TX_D1:
                begin
                    state <= I2C_TX_D2;
                end

                I2C_TX_D2:
                begin
                    state <= I2C_TX_D3;
                end

                I2C_TX_D3:
                begin
                    state <= I2C_TX_D4;
                end

                I2C_TX_D4:
                begin
                    state <= I2C_TX_D5;
                end

                I2C_TX_D5:
                begin
                    state <= I2C_TX_D6;
                end

                I2C_TX_D6:
                begin
                    state <= I2C_TX_D7;
                end

                I2C_TX_D7:
                begin
                    state <= I2C_TX_ACK;
                end

                I2C_TX_ACK: 
                begin
                    state <= I2C_TX_STALL;
                end

                // This could be on address ACK or data ACK
                I2C_TX_STALL:
                begin
                    if (~tx_reg_empty)
                    begin
                        if (ctrl_stop_gen)
                            state <= I2C_STOP;
                        else
                        if (ctrl_restart_gen)
                            state <= I2C_RESTART;
                        else
                        begin
                            if (ctrl_transmit)     state <= I2C_TX_D0;
                            else                 state <= I2C_RX_D0;
                        end
                    end
                    else 
                        state <= I2C_TX_STALL;
                end


                I2C_RX_D0:
                begin
                    state <= I2C_RX_D1;
                end

                I2C_RX_D1:
                begin
                    state <= I2C_RX_D2;
                end

                I2C_RX_D2:
                begin
                    state <= I2C_RX_D3;
                end

                I2C_RX_D3:
                begin
                    state <= I2C_RX_D4;
                end

                I2C_RX_D4:
                begin
                    state <= I2C_RX_D5;
                end

                I2C_RX_D5:
                begin
                    state <= I2C_RX_D6;
                end

                I2C_RX_D6:
                begin
                    state <= I2C_RX_D7;
                end

                I2C_RX_D7: 
                begin
                    state <= I2C_RX_STALL;
                end
                
                I2C_RX_STALL: 
                begin
                    if (~tx_reg_empty) 
                        state <= I2C_RX_ACK;
                    else
                        state <= I2C_RX_STALL;
                end 

                I2C_RX_ACK: 
                begin
                    if (ctrl_nack)     
                    begin
                        if (ctrl_restart_gen)     state <= I2C_RESTART;
                        else                state <= I2C_STOP;
                    end
                    else             
                        state <= I2C_RX_D0;
                end 

                // Need this state to setup the start
                // SDA goes high from whereever it was

                I2C_RESTART: 
                begin
                    state <= I2C_START;
                end 

                // Generate STOP hold time

                I2C_STOP: 
                begin
                    state <= I2C_IDLE; 
                end

                endcase
                end
              end
            end
        end
        else if (Mode == I2C_MODE_SLAVE)
        begin
        
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset) 
                begin
                    state <= I2C_IDLE;
                    byte_complete <= 1'b0;
                end
                else
                begin

                
                /* previous implementation*/
                 /* byte_complete <= 1'b0; */
                
                /* TEST 1*/
                 if (load_dummy) byte_complete <= 1'b0;
                /* End TEST 1*/
            
            
                
                case (state)

                  // Stay in IDLE state until start signal is detected
                  
                I2C_IDLE: 
                begin
                    if (start_sample) 	state <= I2C_RX_DATA;
                    else       			state <= I2C_IDLE;
                end

                // WRITE (address or data)
                // The address state bit is used to distinguish
                // If an address and a match (or any address), stall, otherwise idle

                I2C_RX_DATA: 
                begin
                    if (scl_posedge_detect && count_eq_zero) 
                    begin
                        if (!address | (address & (address_match | ctrl_any_address))) 
                        begin
                            byte_complete <= 1'b1;
                            state <= I2C_RX_STALL;
                        end
                        else               					  
                            state <= I2C_IDLE;
                    end
                    else
                        state <= I2C_RX_DATA;
                end
                
                // Local CPU is reading address and deciding what to do, tx or rx, etc
                // Wait for a dummy write to the tx registe to start either read or write
                // If a write, local CPU can NACK... better not do this on address state

                I2C_RX_STALL: 
                begin
                    if (scl_in == 1'b0 & ~tx_reg_empty) 
                    begin
                    // foo - go to ACK then idle? to be compatible wiht master
                    // would need to also change sda_out computation if this changes (ctrl_nack)
                        if (ctrl_nack) 	state <= I2C_IDLE;
                        else			state <= I2C_RX_ACK;
                    end
                    else
                        state <= I2C_RX_STALL;
                end 

                // ACK the received address or data

                I2C_RX_ACK: 
                begin
                    if (scl_posedge_detect)
                    begin
                        if (ctrl_transmit) 	state <= I2C_TX_DATA; 
                        else				state <= I2C_RX_DATA;
                    end
                    else         			
                        state <= I2C_RX_ACK;
                end 

                // READ
                // 8-bits of data is shifting out

                I2C_TX_DATA: 
                begin
                    if (scl_posedge_detect & count_eq_zero)
                        state <= I2C_TX_ACK;
                    else
                        state <= I2C_TX_DATA;
                end	

                // Check for the ACK from the external master
                // If it's a NACK, then idle

                I2C_TX_ACK: 
                begin
                    /*
                    if (scl_posedge_detect) 
                    begin
                        if (~sda_in)
                        begin
                            byte_complete <= 1'b1;
                            state <= I2C_TX_STALL;
                        end
                        else             
                            state <= I2C_IDLE; 
                    end
                    else
                        state <= I2C_TX_ACK;
                    */
                    
                    if (scl_posedge_detect)  
                    begin
                        byte_complete <= 1'b1;
                        state <= I2C_TX_STALL;
                    end

                end

                // External master ACKed, so it wants more bytes

                I2C_TX_STALL: 
                begin
                    if (scl_in == 1'b0  & ~tx_reg_empty)
                        state <= I2C_TX_DATA;  
                    else
                        state <= I2C_TX_STALL;
                end

                endcase

              end
            end
        end
    endgenerate
    
    
    generate
        if ((Mode == I2C_MODE_MASTER) || (Mode == I2C_MODE_MULTI_MASTER))
        begin
            /**************************************************************************/
            /* Byte complete.                                                         */
            /* Need it to match the Fixed Function implementation.                    */
            /* So instead of a pulse that is capture by sticky register we have a     */
            /* sticky register in PLD that is cleared by GO                           */
            /**************************************************************************/
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset)                                          byte_complete <= 1'b0;
                else if (load_dummy)                                    byte_complete <= 1'b0;
                else if (state == I2C_TX_STALL | state == I2C_RX_STALL) byte_complete <= 1'b1;
            end

            /* LRB (last received bit, which is ACK/NACK from the slave on WRITE command */
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset)                             lrb <= 1'b0;
                else if (clkgen_tc1 & state == I2C_TX_ACK) lrb <= sda_in;
            end
            
            /* Do positive edge detect on contention goes to FSM (to IDLE), and sticky status register */
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset) contention1 <= 1'b0;
                else           contention1 <= contention; 
            end
            
            /*  Busy computation
             * Simple decode of IDLE for single master, use start and stop detect for multi master mode   
             */
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset) bus_busy <= 1'b0;
                else           bus_busy <= (start_detect | bus_busy) & !stop_detect; 
            end
            
            /**************************************************************************
            * Combinational signals                                                  
            ***************************************************************************/
            /* Compute Asynchronous firmware reset */
            assign i2c_reset = ~ctrl_en_master | reset;
            
           

            /* FIX: ������ ����� � ��������� if ��� generate */    
            assign lost_arb = ((Mode == I2C_MODE_MULTI_MASTER) || 
                               (Mode == I2C_MODE_MULTI_MASTER_SLAVE)) ? (contention & ~contention1) : 1'b0;


            /* FIX: ������ ����� � ��������� if ��� generate */    
            assign busy = ((Mode == I2C_MODE_MULTI_MASTER) || 
                           ( Mode == I2C_MODE_MULTI_MASTER_SLAVE)) ? bus_busy : (state != I2C_IDLE);


            /* Contention (multimaster only)
             * Optimized out for simple master
             */
            /* check for contention at the sample points, when the master is transmitting */
            assign contention = clkgen_tc & (txdata | state == I2C_RX_ACK) & (sda_in != sda_out);
            
            /* State decodes for convenience */
            assign rxdata = state == I2C_RX_D0 |
                            state == I2C_RX_D1 |
                            state == I2C_RX_D2 |
                            state == I2C_RX_D3 |
                            state == I2C_RX_D4 |
                            state == I2C_RX_D5 |
                            state == I2C_RX_D6 |
                            state == I2C_RX_D7;

            assign txdata = state == I2C_TX_D0 |
                            state == I2C_TX_D1 |
                            state == I2C_TX_D2 |
                            state == I2C_TX_D3 |
                            state == I2C_TX_D4 |
                            state == I2C_TX_D5 |
                            state == I2C_TX_D6 |
                            state == I2C_TX_D7;

            assign stalled = (state == I2C_TX_STALL | state == I2C_RX_STALL);

        end
        else if (Mode == I2C_MODE_SLAVE)
        begin    
 

            
            // The start detect resets all state in the block, this excepted
            always @(posedge op_clk)
            begin
                start_sample <= start_detect;
            end

            //////////////////////////////////////////////////////////////////////////////
            // LRB (last received bit, which is ACK/NACK from the master on WRITE command !!!!!
            //////////////////////////////////////////////////////////////////////////////
            //
            always @(posedge op_clk or posedge i2c_reset)
            begin
                if (i2c_reset)                                       lrb <= 1'b0;
                else if (scl_posedge_detect  & state == I2C_TX_ACK ) lrb <= sda_in;
            end
            
            // Address bit distinguishs the address reception fwwrom data reception
            // This is sticky until the user writes to continue the transfer
            // 
            always @(posedge op_clk or posedge i2c_reset)
            begin
              if (i2c_reset) address <= 1'b0;
              else    	 	 address <= (start_sample | address) & tx_reg_empty;
            end

            // Compute asynchronous reset
            assign i2c_reset = (~ctrl_en_slave | start_detect ) | reset;
            
            // Compute when to reload bit counter
            assign counter_load = (state == I2C_RX_ACK) | (state == I2C_TX_ACK);

            // Compute when to count
            assign counter_enable = start_sample | shift_enable;

            assign count_eq_zero = (count == 3'b0);
        end
    endgenerate
    
    
    generate
        if ((Mode == I2C_MODE_MASTER) || (Mode == I2C_MODE_MULTI_MASTER))
        begin
            assign shift_enable = clkgen_tc & (rxdata | txdata);    /* Compute when to enable shifting */
            
            /* Compute when to continue transfer, CPU signals this with a write to FIFO F1.
             * This is dummy data, it isn't used, its loaded into A1.
             */    
            assign load_dummy = (~tx_reg_empty & clkgen_tc1);
            
            /* sigmals for control ClkGen module*/
            assign enable_clkgen = (scl_out == scl_in) & ((state == I2C_IDLE & ~tx_reg_empty) | (state != I2C_IDLE));
            assign cs_addr_clkgen = {1'b0, clkgen_tc, enable_clkgen};
            
        end
        else if (Mode == I2C_MODE_SLAVE)
        begin
            assign shift_enable = scl_posedge_detect & (state == I2C_RX_DATA | state == I2C_TX_DATA);
            assign load_dummy = (scl_in == 1'b0) & ~tx_reg_empty & (state == I2C_RX_STALL | state == I2C_TX_STALL);
        end
    endgenerate
        
    assign cs_addr_shifter = {1'b0, load_dummy, shift_enable}; 
    
    /* this signal is used for enable of cy_psoc3_udb_clock_enable_v1_0 */
    assign i2c_enable = ctrl_en_master | ctrl_en_slave;
    
    /*
    generate
        if ((Mode == I2C_MODE_MASTER) || (Mode == I2C_MODE_MULTI_MASTER) || ( Mode == I2C_MODE_MULTI_MASTER_SLAVE))
        begin
            assign d[7:0] = {1'b1, 1'b1, byte_complete, state[4:0]};
            assign d[15:8] = {clock ,clkgen_tc,tx_reg_empty,ctrl_stop_gen, ctrl_restart_gen,ctrl_transmit,ctrl_nack,ctrl_en_master};
            assign d[22:16] = {6'b0, load_dummy};
        end
        else if (Mode == I2C_MODE_SLAVE)
        begin
            assign d[7:0] = {address ,lrb, byte_complete, state[4:0]}; 
            assign d[15:8] = {clock, count_eq_zero, tx_reg_empty, start_detect, load_dummy, ctrl_transmit, ctrl_nack, ctrl_en_master};
            assign d[22:16] = {5'b0,scl_posedge_detect, scl_in, sda_in};
        end
    endgenerate
    */
endmodule /* bI2C_v2_0 */ 
`endif /* bI2C_v2_0_V_ALREADY_INCLUDED */


