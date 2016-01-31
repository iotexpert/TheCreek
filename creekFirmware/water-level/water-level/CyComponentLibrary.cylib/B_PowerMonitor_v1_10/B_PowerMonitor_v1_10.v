/*******************************************************************************
 *
 * FILENAME:  B_PowerMonitor_v1_10.v
 * UM Name:   B_PowerMonitor_v1_10
 *
 * DESCRIPTION:
 *   The Base PowerMonitor User Module is a simple is a simple implementation
 *   to generate output logic on user clock.
 *
 * Todo:
 *
********************************************************************************
* Copyright 2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions,
* disclaimers, and limitations in the end user license agreement accompanying
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef B_POWERMONITOR_V_v1_10_ALREADY_INCLUDED
`else
`define B_POWERMONITOR_V_v1_10_ALREADY_INCLUDED
module B_PowerMonitor_v1_10 (
    input    wire          clock,
    output   wire          fault,
    output   wire          warn,
    output   wire          eoc,
    output   wire[31:0]    pgood_bus
   
);
    localparam DEFAULTNUMCONVERTER = 8'h1;
    localparam DEFAULTPGOODCONFIG  = 1'b0;
    localparam MAXNUMCONVERTERS    = 6'h20;
    localparam WARNSIGNAL          = 8'h0;
    localparam FAULTSIGNAL         = 8'h1;
    localparam EOCSIGNAL           = 8'h2;
    
    parameter NumConverters = DEFAULTNUMCONVERTER;
    parameter PgoodConfig   = DEFAULTPGOODCONFIG;
    

    /************************************************************************** 
    * UDB revisions 
    ***************************************************************************/
    localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);
    localparam CY_UDB_V1 = (!CY_UDB_V0);                         

    /* Clock Enable Block Component instantiation*/
    wire                ClockOutFromEnBlock;
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE))
    clock_enable_block (
                        /* output */.clock_out(ClockOut),
                        /* input */ .clock_in(clock),
                        /* input */ .enable(1'b1)
                        );
   
    /* Contorl register output */
    wire[7:0] control1;
    reg[2:0] control1_reg;
    reg eocReg;
    wire eocSig, eocFinal;
    /**************************************************************************
      Control Register Implementation to generate warn, fault and eoc signals:
     **************************************************************************/
    generate
    if(CY_UDB_V0)begin : AsyncCtl
        cy_psoc3_control #(.cy_force_order(`TRUE))
        ctrlreg1(
                /* output 07:00] */.control(control1));
    
        /* drive the contorl output using a flip-flop with input clock */
        always @(posedge ClockOut)
        begin
            control1_reg[FAULTSIGNAL] <= control1[FAULTSIGNAL];
            control1_reg[WARNSIGNAL] <= control1[WARNSIGNAL];
            control1_reg[EOCSIGNAL] <= control1[EOCSIGNAL];
        end
        
        /* output assignment */
        assign fault = control1_reg[FAULTSIGNAL];
        assign warn  = control1_reg[WARNSIGNAL];
        
    end
    else begin : SyncCtl
        /* Add support of sync mode for PSoC3 Rev */
        /*******************************************************************
         * The clock to operate Control Reg for PSOC3 ES3 must be 
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

        cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFB))
        ctrlreg1 (
            /* input          */ .clock(Clk_Ctl_i),
            /* output [07:00] */ .control(control1)
        );
        /* drive the contorl output using a flip-flop with input clock */
        always @(posedge ClockOut)
        begin
            control1_reg[EOCSIGNAL] <= control1[EOCSIGNAL];
        end
        
        /* output assignment */
        assign fault = control1[FAULTSIGNAL];
        assign warn  = control1[WARNSIGNAL];
    end
    endgenerate

        /* EOC output signal generation */
        assign eocSig = control1[EOCSIGNAL] ^ control1_reg[EOCSIGNAL];
        always @(posedge ClockOut)
        begin
            eocReg <= eocSig;
        end
        
        assign eoc = eocReg;
    
    /* Pgood generation */
    /* Contorl register output */
    wire[31:0] pgood_sig; 
    reg[31:0] pgood_reg;

    /**************************************************************************
      Control Register Implementation to generate pgood signal:
     **************************************************************************/
    generate
    if(CY_UDB_V0)
    begin : Async
        if (1)
        begin : PM_1_8
        cy_psoc3_control #(.cy_force_order(`TRUE)) Ctrl1
        (
             /* output 07:00] */.control(pgood_sig[7:0])
        );
        always @(posedge ClockOut)
        begin
            pgood_reg[7:0] <= pgood_sig[7:0];
        end
        end
        
        if(NumConverters > 8)
        begin : PM_9_16
            cy_psoc3_control #(.cy_force_order(`TRUE)) Ctrl2
            (
                /* output 07:00] */.control(pgood_sig[15:8])
            );
            always @(posedge ClockOut)
            begin
                pgood_reg[15:8] <= pgood_sig[15:8];
            end
        end
    
        if(NumConverters > 16)
        begin : PM_17_24
            cy_psoc3_control #(.cy_force_order(`TRUE)) Ctrl3
            (
                    /* output 07:00] */.control(pgood_sig[23:16]));
            always @(posedge ClockOut)
            begin
                pgood_reg[23:16] <= pgood_sig[23:16];
            end
        end
        
        if(NumConverters > 24)
        begin : PM_25_32
            cy_psoc3_control #(.cy_force_order(`TRUE)) Ctrl4
            (
                    /* output 07:00] */.control(pgood_sig[31:24]));
            always @(posedge ClockOut)
            begin
                pgood_reg[31:24] <= pgood_sig[31:24];
            end
        end
    end
    else begin : Sync
        /* Add support of sync mode for PSoC3 Rev */
        /*******************************************************************
         * The clock to operate Control Reg for PSOC3 ES3 must be 
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

        cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFF)) PM_1_8_Ctrl1
        (
            /* input          */ .clock(Clk_Ctl_i),
            /* output [07:00] */ .control(pgood_reg[7:0])
        );
        
        if(NumConverters > 8)
        begin: PM_9_16
            cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFF)) Ctrl2
            (
                /* input          */ .clock(Clk_Ctl_i),
                /* output [07:00] */ .control(pgood_reg[15:8])
            );
        end
        
        if(NumConverters > 16)
        begin: PM_17_24
            cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFF)) Ctrl3
            (
                /* input          */ .clock(Clk_Ctl_i),
                /* output [07:00] */ .control(pgood_reg[23:16])
            );
        end
        
        if(NumConverters > 24)
        begin: PM_25_32
            cy_psoc3_control #(.cy_force_order(`TRUE), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'hFF)) Ctrl4
            (
                /* input          */ .clock(Clk_Ctl_i),
                /* output [07:00] */ .control(pgood_reg[31:24])
            );
        end
    end
    endgenerate
    
    generate
    if(PgoodConfig == DEFAULTPGOODCONFIG)
    begin : PGOODSINGLE
        assign pgood_bus[0] = (| pgood_reg[(NumConverters-1):0]);
        assign pgood_bus[31:1] = 31'b0;    
    end
    else
    begin : PGOODBUS                                            
        if(NumConverters < MAXNUMCONVERTERS)
        begin
            assign pgood_bus[(MAXNUMCONVERTERS-1):0] = {{(MAXNUMCONVERTERS- NumConverters){1'b0}}, pgood_reg[NumConverters-1:0]};
        end
        else
        begin
            assign pgood_bus[(MAXNUMCONVERTERS-1):0] = pgood_reg[(MAXNUMCONVERTERS-1):0];
        end
    end
    endgenerate

endmodule


`endif /*B_POWERMONITOR_V_v1_10_ALREADY_INCLUDED*/

