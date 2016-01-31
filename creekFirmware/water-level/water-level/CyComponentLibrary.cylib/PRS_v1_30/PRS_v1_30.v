/*******************************************************************************
* File Name:  PRS_v1_30.v 
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description: 
*  This file provides a top level model of the PRS componnent
*  defining and all of the necessary interconnect.
* 
* Note: 
*  None
********************************************************************************
*                 Control and Status Register definitions
******************************************************************************** 
*
* Control Register Definition
*
*  ctrl_enable => 0 = disable PRS
*                 1 = enable PRS
*
*  api_clock   => Software clock for PRS. Firstly must be set "1", 
*                 after that - "0". This is emulate rising edge of clock signal.
*
*  dff_reset   => Software reset for dtrig. Firstly must be set "1", 
*                 after that - "0". This is emulate rising edge for reset 
*                 triggers.
*
******************************************************************************** 
*                 Data Path register definitions
******************************************************************************** 
*  INSTANCE NAME:  DatapathName 
*  DESCRIPTION:
*  REGISTER USAGE:
*    F0 => na
*    F1 => na
*    D0 => Lower half of polynomial value
*    D1 => Upper half of polynomial value
*    A0 => Lower half of seed value 
*    A1 => Upper half of seed value  
*
******************************************************************************** 
*               I*O Signals:   
******************************************************************************** 
*  IO SIGNALS: 
*   di            input        Input Data
*   clock         input        Data clock
*   bitstream     output       Bitstream
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/

`include "cypress.v"
`ifdef PRS_v1_30_V_ALREADY_INCLUDED
`else
`define PRS_v1_30_V_ALREADY_INCLUDED

module PRS_v1_30
(
    input wire clock,
    input wire enable,
    output wire bitstream
);

   /**************************************************************************/
   /* Parameters                                                             */
   /**************************************************************************/
   localparam PRS_8_BIT  = 7'd8;
   localparam PRS_16_BIT = 7'd16;
   localparam PRS_24_BIT = 7'd24;
   localparam PRS_32_BIT = 7'd32;
   localparam PRS_40_BIT = 7'd40;
   localparam PRS_48_BIT = 7'd48;
   localparam PRS_56_BIT = 7'd56;
   localparam PRS_64_BIT = 7'd64;
   parameter [6:0] Resolution = PRS_8_BIT;    
   
   localparam PRS_RUNMODE_CLOCKED         = 1'd0;
   localparam PRS_RUNMODE_API_SINGLE_STEP = 1'd1;
   parameter RunMode = PRS_RUNMODE_CLOCKED;

   /* Internal signals */
   wire clk;
   wire so, so_reg, ci, si, cmsb;
   wire dcfg, dcfg_b, ci_temp, sc_temp, nclk, si_final, so_chain;
   wire si_d, si_c, si_b, si_a;
   wire so_d, so_c, so_b, so_a;
   wire [7:0] sc_out, sc_out_a, sc_out_b, sc_out_c, sc_out_d;
   wire [2:0] cs_addr;   

   /**************************************************************************/
   /* Control Register Implementation                                        */
   /**************************************************************************/

   /* Control Register Bits (Bits 7-3 are unused )*/
    localparam PRS_CTRL_ENABLE      = 2'h0;   /* Enable PRS */             
    localparam PRS_CTRL_RISING_EDGE = 2'h1;   /* API single step */
    localparam PRS_CTRL_DFF_RESET   = 2'h2;   /* Software reset for dtrig */ 
   
    wire [7:0] control;                       /* Control Register Output */    

    /* Control Signals */
    wire ctrl_enable = control[PRS_CTRL_ENABLE];
    wire api_clock   = control[PRS_CTRL_RISING_EDGE];
    wire dff_reset   = control[PRS_CTRL_DFF_RESET];
   
   cy_psoc3_control #(.cy_force_order(1)) CtrlReg(
        /*  output   [07:00]    */  .control(control)
   );  

   /**************************************************************************/
   /* Instantiate the data path elements                                     */
   /**************************************************************************/

   localparam [2:0] dpPOVal = (Resolution <= PRS_8_BIT)  ? (Resolution - 1): 
                              (Resolution <= PRS_16_BIT) ? (Resolution - 9):
                              (Resolution <= PRS_24_BIT) ? (Resolution - 17):
                              (Resolution <= PRS_32_BIT) ? (Resolution - 25):
                              (Resolution <= PRS_40_BIT) ? (Resolution - 33):
                              (Resolution <= PRS_48_BIT) ? (Resolution - 41):
                              (Resolution <= PRS_56_BIT) ? (Resolution - 49):
                              (Resolution <= PRS_64_BIT) ? (Resolution - 57): 0;

   localparam [2:0] dpMsbVal = (Resolution < PRS_8_BIT ) ? (Resolution - 1) : 7;
     
   wire ctrl_enable_final;

   assign ctrl_enable_final = ctrl_enable & enable;

   generate
      if (RunMode == PRS_RUNMODE_CLOCKED)
      begin
        assign clk = clock;
      end
      else
      begin
        assign clk = api_clock;
      end
   endgenerate


   localparam dpconfig0 ={
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    `CS_CMP_SEL_CFGA, /*CS_REG0 Comment: */
    `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_D0,
    `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
    `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:LSB */
    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
    `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    `CS_CMP_SEL_CFGA, /*CS_REG2 Comment: */
    `CS_ALU_OP__XOR, `CS_SRCA_A1, `CS_SRCB_D1,
    `CS_SHFT_OP___SL, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
    `CS_FEEDBACK_ENBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
    `CS_CMP_SEL_CFGA, /*CS_REG3 Comment:MSB */
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
      8'hFF, 8'hFF,    /*SC_REG5    Comment: */
    `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_CHAIN,
    `SC_CI_A_ROUTE, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
    `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
    `SC_SI_A_ROUTE, /*SC_REG6 Comment: */
    `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
    1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
    `SC_MSB_ENBL, dpMsbVal, `SC_MSB_NOCHN,
    `SC_FB_NOCHN, `SC_CMP1_NOCHN,
    `SC_CMP0_NOCHN, /*SC_REG7 Comment: */
     10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
    `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_ENBL,
    `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
    };
   
    generate
        if (Resolution <= PRS_8_BIT) 
        begin
            assign cs_addr = {1'b0, 1'b0, ctrl_enable_final};
            assign ci = cmsb;
            assign bitstream = cmsb;    /* Bitstream output if Time Multiplexing not used*/
        end
        else 
        begin
            assign cs_addr = {1'b0, dcfg, ctrl_enable_final};

            /******************************************************************/
            /* Time Multiplexing Logic                                        */
            /******************************************************************/   
    
            /* sync so */            
            PRS_dtrig_v1_0 sync(
                /*  output      */  .q(so_reg),
                /*  input       */  .d(so),
                /*  input       */  .reset_val(1'b1),  
                /*  input       */  .reset(dff_reset),  
                /*  input       */  .clk(clk)
            );
     
            assign nclk = ~clk;
            
            /* dcfg - dynamic configuration */            
            PRS_dtrig_v1_0 d0(
                /*  output      */  .q(dcfg),
                /*  input       */  .d(dcfg_b),
                /*  input       */  .reset_val(1'b0),  
                /*  input       */  .reset(dff_reset),  
                /*  input       */  .clk(nclk)
            );
              
            assign dcfg_b = ~dcfg;
            
            /* ci */
            PRS_dtrig_v1_0 d1(
                /*  output      */  .q(ci_temp),
                /*  input       */  .d(ci),
                /*  input       */  .reset_val(1'b1),  
                /*  input       */  .reset(dff_reset),  
                /*  input       */  .clk(nclk)
            );
              
            assign ci = dcfg ? sc_out[dpPOVal] : ci_temp;

            /* si */ 
            PRS_dtrig_v1_0 d2(
                /*  output      */  .q(sc_temp),
                /*  input       */  .d(sc_out[dpPOVal]),
                /*  input       */  .reset_val(1'b1),  
                /*  input       */  .reset(dff_reset),  
                /*  input       */  .clk(nclk)
            );
            
            assign si = dcfg ? so_reg : sc_temp;
            
            /* Bitstream output */
            assign bitstream = ci_temp;
        end 
    endgenerate
   
    /* Chain Datapathes*/
    generate
        if (Resolution <= PRS_8_BIT)
        begin 
              assign si = cmsb;
        end
        else if (Resolution <= PRS_16_BIT)
        begin
            assign so = so_a;  
            assign sc_out = sc_out_a;
        end
        else if (Resolution <= PRS_24_BIT)
        begin
            assign si_b = dcfg ? si : so_a;
            assign so = so_b;
            assign sc_out = sc_out_b;
        end
        else if (Resolution <= PRS_32_BIT)
        begin
            assign si_b = so_a;
            assign so = so_b;
            assign sc_out = sc_out_b;
        end    
        else if (Resolution <= PRS_40_BIT)
        begin  
            assign si_c = so_b;
            assign si_b = dcfg ? si : so_a;
            assign so = so_c;
            assign sc_out = sc_out_c;
        end
        else if (Resolution <= PRS_48_BIT) 
        begin
            assign si_c = so_b;
            assign si_b = so_a;
            assign so = so_c;
            assign sc_out = sc_out_c;
        end
        else if (Resolution <= PRS_56_BIT)
        begin
            assign si_d = so_c;
            assign si_c = so_b;
            assign si_b = dcfg ? si : so_a;
            assign so = so_d;
            assign sc_out = sc_out_d;
        end
        else if (Resolution <= PRS_64_BIT) 
        begin
            assign si_d = so_c;
            assign si_c = so_b;
            assign si_b = so_a;
            assign so = so_d;
            assign sc_out = sc_out_d;
        end
    endgenerate

        
    generate
        if(Resolution > PRS_48_BIT) 
        begin : b3 
            cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) PRSdp_d(
                /*  input                   */  .clk(clk),        
                /*  input   [02:00]         */  .cs_addr(cs_addr),
                /*  input                   */  .route_si(si_d),      
                /*  input                   */  .route_ci(ci),    
                /*  input                   */  .f0_load(1'b0),     
                /*  input                   */  .f1_load(1'b0),     
                /*  input                   */  .d0_load(1'b0),     
                /*  input                   */  .d1_load(1'b0),     
                /*  output                  */  .ce0(),             
                /*  output                  */  .cl0(),             
                /*  output                  */  .z0(),              
                /*  output                  */  .ff0(),             
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(), 
                /*  output                  */  .cmsb(), 
                /*  output                  */  .so(so_d),  
                /*  output                  */  .f0_bus_stat(),     
                /*  output                  */  .f0_blk_stat(),     
                /*  output                  */  .f1_bus_stat(),     
                /*  output                  */  .f1_blk_stat(),
        
                /* input                    */  .ci(1'b0),     
                /* output                   */  .co(),         
                /* input                    */  .sir(1'b0),    
                /* output                   */  .sor(),        
                /* input                    */  .sil(1'b0),    
                /* output                   */  .sol(),        
                /* input                    */  .msbi(1'b0),   
                /* output                   */  .msbo(),       
                /* input [01:00]            */  .cei(2'b0),    
                /* output [01:00]           */  .ceo(),        
                /* input [01:00]            */  .cli(2'b0),    
                /* output [01:00]           */  .clo(),        
                /* input [01:00]            */  .zi(2'b0),     
                /* output [01:00]           */  .zo(),         
                /* input [01:00]            */  .fi(2'b0),     
                /* output [01:00]           */  .fo(),         
                /* input [01:00]            */  .capi(2'b0),   
                /* output [01:00]           */  .capo(),       
                /* input                    */  .cfbi(1'b0),   
                /* output                   */  .cfbo(),       
                /* input [07:00]            */  .pi(8'b0),     
                /* output [07:00]           */  .po(sc_out_d[7:0])         
            );
        end 
        if(Resolution > PRS_32_BIT) 
        begin : b2
            cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) PRSdp_c(
                /*  input                   */  .clk(clk),        
                /*  input   [02:00]         */  .cs_addr(cs_addr),
                /*  input                   */  .route_si(si_c),      
                /*  input                   */  .route_ci(ci),    
                /*  input                   */  .f0_load(1'b0),     
                /*  input                   */  .f1_load(1'b0),     
                /*  input                   */  .d0_load(1'b0),     
                /*  input                   */  .d1_load(1'b0),     
                /*  output                  */  .ce0(),             
                /*  output                  */  .cl0(),             
                /*  output                  */  .z0(),              
                /*  output                  */  .ff0(),             
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(), 
                /*  output                  */  .cmsb(), 
                /*  output                  */  .so(so_c),  
                /*  output                  */  .f0_bus_stat(),     
                /*  output                  */  .f0_blk_stat(),     
                /*  output                  */  .f1_bus_stat(),     
                /*  output                  */  .f1_blk_stat(),
        
                /* input                    */  .ci(1'b0),     
                /* output                   */  .co(),         
                /* input                    */  .sir(1'b0),    
                /* output                   */  .sor(),        
                /* input                    */  .sil(1'b0),    
                /* output                   */  .sol(),        
                /* input                    */  .msbi(1'b0),   
                /* output                   */  .msbo(),       
                /* input [01:00]            */  .cei(2'b0),    
                /* output [01:00]           */  .ceo(),        
                /* input [01:00]            */  .cli(2'b0),    
                /* output [01:00]           */  .clo(),        
                /* input [01:00]            */  .zi(2'b0),     
                /* output [01:00]           */  .zo(),         
                /* input [01:00]            */  .fi(2'b0),     
                /* output [01:00]           */  .fo(),         
                /* input [01:00]            */  .capi(2'b0),   
                /* output [01:00]           */  .capo(),       
                /* input                    */  .cfbi(1'b0),   
                /* output                   */  .cfbo(),       
                /* input [07:00]            */  .pi(8'b0),     
                /* output [07:00]           */  .po(sc_out_c[7:0])          
            );            
        end 
        if(Resolution > PRS_16_BIT) 
        begin : b1
            cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) PRSdp_b(
                /*  input                   */  .clk(clk),        
                /*  input   [02:00]         */  .cs_addr(cs_addr),
                /*  input                   */  .route_si(si_b),      
                /*  input                   */  .route_ci(ci),    
                /*  input                   */  .f0_load(1'b0),     
                /*  input                   */  .f1_load(1'b0),     
                /*  input                   */  .d0_load(1'b0),     
                /*  input                   */  .d1_load(1'b0),     
                /*  output                  */  .ce0(),             
                /*  output                  */  .cl0(),             
                /*  output                  */  .z0(),              
                /*  output                  */  .ff0(),             
                /*  output                  */  .ce1(),
                /*  output                  */  .cl1(),
                /*  output                  */  .z1(),
                /*  output                  */  .ff1(),
                /*  output                  */  .ov_msb(),
                /*  output                  */  .co_msb(), 
                /*  output                  */  .cmsb(), 
                /*  output                  */  .so(so_b),  
                /*  output                  */  .f0_bus_stat(),     
                /*  output                  */  .f0_blk_stat(),     
                /*  output                  */  .f1_bus_stat(),     
                /*  output                  */  .f1_blk_stat(),
        
                /* input                    */  .ci(1'b0),     
                /* output                   */  .co(),         
                /* input                    */  .sir(1'b0),    
                /* output                   */  .sor(),        
                /* input                    */  .sil(1'b0),    
                /* output                   */  .sol(),        
                /* input                    */  .msbi(1'b0),   
                /* output                   */  .msbo(),       
                /* input [01:00]            */  .cei(2'b0),    
                /* output [01:00]           */  .ceo(),       
                /* input [01:00]            */  .cli(2'b0),    
                /* output [01:00]           */  .clo(),        
                /* input [01:00]            */  .zi(2'b0),     
                /* output [01:00]           */  .zo(),         
                /* input [01:00]            */  .fi(2'b0),     
                /* output [01:00]           */  .fo(),         
                /* input [01:00]            */  .capi(2'b0),   
                /* output [01:00]           */  .capo(),       
                /* input                    */  .cfbi(1'b0),   
                /* output                   */  .cfbo(),       
                /* input [07:00]            */  .pi(8'b0),     
                /* output [07:00]           */  .po(sc_out_b[7:0])          
            );
        end 
    endgenerate
   

    cy_psoc3_dp #(.cy_dpconfig(dpconfig0)) PRSdp_a(
         /*  input                   */  .clk(clk),        
         /*  input   [02:00]         */  .cs_addr(cs_addr),
         /*  input                   */  .route_si(si),
         /*  input                   */  .route_ci(ci),
         /*  input                   */  .f0_load(1'b0),
         /*  input                   */  .f1_load(1'b0),
         /*  input                   */  .d0_load(1'b0),
         /*  input                   */  .d1_load(1'b0),
         /*  output                  */  .ce0(),
         /*  output                  */  .cl0(),             
         /*  output                  */  .z0(),              
         /*  output                  */  .ff0(),             
         /*  output                  */  .ce1(),
         /*  output                  */  .cl1(),
         /*  output                  */  .z1(),
         /*  output                  */  .ff1(),
         /*  output                  */  .ov_msb(),
         /*  output                  */  .co_msb(), 
         /*  output                  */  .cmsb(cmsb), 
         /*  output                  */  .so(so_a),  
         /*  output                  */  .f0_bus_stat(),     
         /*  output                  */  .f0_blk_stat(),     
         /*  output                  */  .f1_bus_stat(),     
         /*  output                  */  .f1_blk_stat(),

         /* input                    */  .ci(1'b0),     
         /* output                   */  .co(),         
         /* input                    */  .sir(1'b0),    
         /* output                   */  .sor(),        
         /* input                    */  .sil(1'b0),    
         /* output                   */  .sol(),        
         /* input                    */  .msbi(1'b0),   
         /* output                   */  .msbo(),       
         /* input [01:00]            */  .cei(2'b0),    
         /* output [01:00]           */  .ceo(),        
         /* input [01:00]            */  .cli(2'b0),    
         /* output [01:00]           */  .clo(),        
         /* input [01:00]            */  .zi(2'b0),     
         /* output [01:00]           */  .zo(),         
         /* input [01:00]            */  .fi(2'b0),     
         /* output [01:00]           */  .fo(),         
         /* input [01:00]            */  .capi(2'b0),   
         /* output [01:00]           */  .capo(),       
         /* input                    */  .cfbi(1'b0),   
         /* output                   */  .cfbo(),       
         /* input [07:00]            */  .pi(8'b0),     
         /* output [07:00]           */  .po(sc_out_a[7:0])          
    );
   
endmodule   /* PRS_v1_30 */

`endif      /* PRS_v1_30_V_ALREADY_INCLUDED */


`ifdef PRS_dtrig_v1_0_V_ALREADY_INCLUDED
`else
`define PRS_dtrig_v1_0_V_ALREADY_INCLUDED
    /******************************************************************/
    /* D-Triger                                                       */
    /******************************************************************/ 
    module PRS_dtrig_v1_0
    (
        output reg q,
        input wire d,
        input wire reset_val,
        input wire reset,
        input wire clk
    );
        
    always @(posedge clk or posedge reset)
    begin
        if (reset)
        begin
            q = reset_val;
        end
        else
        begin
            q = d;
        end
    end 
    endmodule   /* PRS_dtrig_v1_0 */
`endif          /* PRS_dtrig_v1_0_V_ALREADY_INCLUDED */