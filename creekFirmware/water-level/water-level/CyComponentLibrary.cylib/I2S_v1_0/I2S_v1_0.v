/*******************************************************************************
* File Name: I2S_v1_0.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* The I2S module is inter-IC sound interface implementation done with UDB's. It 
* operates as bus master in both direction, Rx and Tx respectively. I2S module
* has setable word select period, data resolution, support Interrupts and DMA
* operations
*
*------------------------------------------------------------------------------
*                 Control and Status Register definitions
*------------------------------------------------------------------------------
*
*  Control Register Definition
*  +=====+-------+-------+-------+--------+--------+--------+--------+--------+
*  | Bit |   7   |   6   |   5   |   4    |   3    |   2    |   1    |   0    |
*  +=====+-------+-------+-------+--------+--------+--------+--------+--------+
*  |Desc |        unused         | enable | reset  |CntrLoad|rxenable|txenable| 
*  +=====+-------+-------+-------+--------+--------+--------+--------+--------+
*
*    txenable     =>   0 = disable Tx Direction
*                      1 = enable Tx Direction
*                      
*    rxenable     =>   0 = disable Tx Direction
*                      1 = enable Tx Direction           
*
*    CntrLoad       => 1 = load counter period value
*
*    reset        =>   0 = un-reset
*                      1 = reset I2S component
*
*    enable       =>   0 = disable I2S component
*                      1 = enable I2S component
*    
*  Tx Interrupt Status Register Definition
*  +=======+---------+------+------+------+-----+---------+---------+---------+
*  |  Bit  |    7    |  6   |  5   |  4   |  3  |    2    |    1    |    0    |
*  +=======+---------+------+------+------+-----+---------+---------+---------+
*  | Desc  |interrupt|          unused          |f1_n_full|f0_n_full|underflow|
*  +=======+---------+------+------+------+-----+---------+---------+---------+
*
*    f1_n_full     =>  0 = Tx fifo1 not full event has not occured 
*                      1 = Tx fifo1 not full event has occured
*
*    f0_n_full     =>  0 = Tx fifo0 not full event has not occured
*                      1 = Tx fifo0 not full event has occured
*
*    underflow     =>  0 = Tx fifo underflow event has not occured
*                      1 = Tx fifo underflow event has occured
*
*  Rx Interrupt Status Register Definition
*  +=======+---------+------+------+------+------+---------+---------+--------+
*  |  Bit  |   7     |  6   |  5   |  4   |  3   |    2    |    1    |    0   |
*  +=======+---------+------+------+------+------+---------+---------+--------+
*  | Desc  |interrupt|         unused            |f1_n_empt|f0_n_empt|overflow|      
*  +=======+---------+------+------+------+------+---------+---------+--------+
*
*    f1_n_empt     =>  0 = Rx fifo1 not empty event has not occured 
*                      1 = Rx fifo1 not empty event has occured
*
*    f0_n_empt     =>  0 = Rx fifo0 not empty event has not occured
*                      1 = Rx fifo0 not empty event has occured
*
*    overflow      =>  0 = Rx fifo overflow event has not occured
*                      1 = Rx fifo overflow event has occured
********************************************************************************
* Data Path register definitions
********************************************************************************
* I2S: dpTx
* DESCRIPTION: dpTx is used to implement the Tx direction of I2S component
* REGISTER USAGE:
* F0 => Tx left channel FIFO
* F1 => Tx right channel FIFO
* D0 => not used
* D1 => not used
* A0 => shift operation source during data transition
* A1 => shift operation source during data transition
*
********************************************************************************
* I2S: dpRx
* DESCRIPTION:
* REGISTER USAGE: dpRx is used to implement the Rx direction of I2S component 
* F0 => Rx left channel FIFO
* F1 => Rx right channel FIFO
* D0 => not used
* D1 => not used
* A0 => shift operation source during data receiving
* A1 => shift operation source during data receiving
*
********************************************************************************
* I*O Signals:
********************************************************************************
*    name              direction       Description
*    SCK               output          I2S clock line                      
*    SDO               output          I2S output data line                
*    WS                output          I2S word select line                
*    SDI               input           I2S input data line                 
*    clock             input           System clock                        
*    rx_DMA0           output          Rx direction DMA request for FIFO 0 
*    rx_DMA1           output          Rx direction DMA request for FIFO 1 
*    rx_interrupt      output          Rx direction interrupt              
*    tx_DMA0           output          Tx direction DMA request for FIFO 0 
*    tx_DMA1           output          Tx direction DMA request for FIFO 1 
*    tx_interrupt      output          Tx direction interrupt              
*
********************************************************************************
* Copyright 2008-2009, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/




`include "cypress.v"
`ifdef I2S_V_v1_0_ALREADY_INCLUDED
`else
`define I2S_V_v1_0_ALREADY_INCLUDED

module I2S_v1_0 (
    output  wire    SCK,             /* I2S clock line                      */
    output  wire    SDO,             /* I2S output data line                */
    output  wire    WS,              /* I2S word select line                */
    input   wire    SDI,             /* I2S input data line                 */
    input   wire    clock,           /* System clock                        */
    output  wire    rx_DMA0,         /* Rx direction DMA request for FIFO 0 */
    output  wire    rx_DMA1,         /* Rx direction DMA request for FIFO 1 */
    output  wire    rx_interrupt,    /* Rx direction interrupt              */
    output  wire    tx_DMA0,         /* Tx direction DMA request for FIFO 0 */
    output  wire    tx_DMA1,         /* Tx direction DMA request for FIFO 1 */
    output  wire    tx_interrupt     /* Tx direction interrupt              */
);
    
    /**************************************************************************/
    /*              Parameters                                                */
    /**************************************************************************/
    parameter  [6:0] DataBits           = 7'd16;
    parameter  [6:0] WordSelect         = 7'd32;
    parameter  [1:0] Direction             = 2'd3;
    parameter  [5:0] InterruptSource    = 6'h00;
    parameter  RxDataInterleaving       = 1;
    parameter  RxDMA_present            = 0;
    parameter  TxDataInterleaving       = 1;
    parameter  TxDMA_present            = 0;
        
    localparam [6:0] ChannelResolution = WordSelect/2; 
    localparam [6:0] I2SBitCounterPeriod = WordSelect-1;               
    
    /**************************************************************************
    * MSB and LSB position definition. MSB and LSB are used to determine unused
    * bits for each direction. Any unused bits will be ignored on Tx and will 
    * 0 on Rx.
    **************************************************************************/
        
    localparam [6:0] MsbLeft = I2SBitCounterPeriod - 1;
    localparam [6:0] LsbLeft = I2SBitCounterPeriod - DataBits;
    
    localparam [6:0] MsbRight = MsbLeft - ChannelResolution;
    localparam [6:0] LsbRight = LsbLeft - ChannelResolution;
        
    /**************************************************************************/
    /* Control Register Implementation                                        */
    /**************************************************************************/
    wire [7:0] ctrl;
    /* Control Register bit location (bits 5-7 are unused) */
    localparam I2S_CTRL_TX_ENABLE       = 3'd0;
    localparam I2S_CTRL_RX_ENABLE       = 3'd1;
    localparam I2S_CTRL_BITCOUNTER_LOAD = 3'd2;
    localparam I2S_CTRL_RESET           = 3'd3;
    localparam I2S_CTRL_ENABLE          = 3'd4;
    
    cy_psoc3_control #( .cy_force_order(1))
    ControlReg(
        /* output [07:00] */ .control(ctrl)
    );    
    wire txenable     = ctrl[I2S_CTRL_TX_ENABLE];        //txenable
    wire rxenable     = ctrl[I2S_CTRL_RX_ENABLE];        //rxenable
    wire counterLoad  = ctrl[I2S_CTRL_BITCOUNTER_LOAD];  //counter7 load signal
    wire reset          = ctrl[I2S_CTRL_RESET];            //reset signal
    wire enable          = ctrl[I2S_CTRL_ENABLE];           //enable I2S
    
    
    /**************************************************************************/
    /* 7-bit Down Counter implementation                                      */
    /**************************************************************************/
    wire [6:0]  count; 
    cy_psoc3_count7 #(.cy_period(I2SBitCounterPeriod),.cy_route_ld(1),.cy_route_en(1))
    BitCounter(
        /*  input             */  .clock(clock),
        /*  input             */  .reset(reset),
        /*  input             */  .load(counterLoad),
        /*  input             */  .enable(enable),
        /*  output  [06:00]   */  .count(count),
        /*  output            */  .tc()
    );
    
    assign SCK = ~clock & enable;    /* I2S clock generation  */
    wire channel_sel;
    localparam WordSelect16bit     = 8'd16;
    localparam WordSelect32bit     = 8'd32;
    localparam WordSelect48bit     = 8'd48;
    localparam WordSelect64bit     = 8'd64;

    generate  /* Channel Select generation based on Bit Counter value and WordSelect parameter value */
        if(WordSelect == WordSelect16bit)
        begin
            assign channel_sel = ~count[3];
        end
        else
        begin
            if (WordSelect == WordSelect32bit)
            begin
                assign channel_sel = ~count[4];
            end
            else
            begin
                if (WordSelect == WordSelect48bit)
                begin
                    assign channel_sel = ~(count[5] | count[4] & count[3]);                    
                end
                else
                begin
                    assign channel_sel = ~count[5];
                end
            end
        end
    endgenerate  /* Channel Select generation */
    
    assign WS = channel_sel & enable;  /* Word Select Output Generation */
    
    localparam RxPresent        = 2'd1;
    localparam TxPresent        = 2'd2;
    localparam RxAndTxPresent   = 2'd3;
    /****************************************************************************/
    /*                      TX direction implementation                         */
    /****************************************************************************/
    localparam TxDirectionPresent = (Direction==TxPresent || Direction==RxAndTxPresent); 
    generate
    if(TxDirectionPresent)
    begin: Tx
    /****************************************************************************/
    /*        Tx Status Register Implementation                                 */
    /****************************************************************************/
        /*    TX Status Register bit location (bits 6-3 unused)    */
        localparam TX_FIFO_UNDERFLOW  = 3'd0;
        localparam TX_FIFO_0_NOT_FULL = 3'd1;
        localparam TX_FIFO_1_NOT_FULL = 3'd2;
        localparam [6:0] TxIntSource = {4'b0, InterruptSource[2:0]};
        wire tx_f0_not_full_stat;    /* Tx FIFO 0 not full status bit */ 
        wire tx_f1_not_full_stat;     /* Tx FIFO 1 not full status bit */
        wire tx_f0_empty_stat;       /* Tx FIFO 0 empty status bit */
        wire tx_f1_empty_stat;       /* Tx FIFO 1 empty status bit */
        wire [6:0] tx_status;        /* status register input */    
        wire tx_underflow;
        assign tx_status[TX_FIFO_UNDERFLOW]  = tx_underflow;
        assign tx_status[TX_FIFO_0_NOT_FULL] = tx_f0_not_full_stat;
        assign tx_status[TX_FIFO_1_NOT_FULL] = tx_f1_not_full_stat;
        assign tx_status[6:3] = 4'b0;        /* Status register unused bits */ 
    /* Instantiate the status register */ 
        cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'b0000111), 
            .cy_int_mask(TxIntSource)) 
        tx_sts_reg(
        /* input          */  .clock(clock),
        /* input  [06:00] */  .status(tx_status),
        /* output         */  .interrupt(tx_interrupt)  
       );
   
  /****************************************************************************/
  /*        Tx DMA Request Outputs Implementation                             */
  /****************************************************************************/   
        assign tx_DMA0 = tx_f0_not_full_stat;
        assign tx_DMA1 = tx_f1_not_full_stat;
  
        /* Define channel sources accordingly to data interleaving. If data 
        * are interleaved then FIFO 0 is source for LEFT and RIGHT channels, 
        * else FIFO 0 is source for LEFT channel and FIFO 1 for RIGHT channel */
        
        /* TX State Machine States: */
        localparam I2S_TX_STATE_ZERO      = 3'd0;
        localparam I2S_TX_STATE_SHL_LEFT  = 3'd1;
        localparam I2S_TX_STATE_LD_LEFT   = 3'd2;
        localparam I2S_TX_STATE_LD_RIGHT  = 3'd3; 
        localparam I2S_TX_STATE_SHL_RIGHT = 3'd4;
        
        reg [2:0] tx_state;
        always @(posedge clock or posedge reset)
        begin
            if(reset) tx_state <= I2S_TX_STATE_ZERO;
            else
            begin
                case(tx_state)
                I2S_TX_STATE_ZERO: 
                if((txenable == 1'b1) && (count == 6'd0))
                    tx_state <= I2S_TX_STATE_LD_LEFT;
                else 
                    tx_state <= I2S_TX_STATE_ZERO;
                I2S_TX_STATE_LD_LEFT:
                begin
                    tx_state <= I2S_TX_STATE_SHL_LEFT;
                end
                I2S_TX_STATE_LD_RIGHT:
                begin   
                    tx_state <= I2S_TX_STATE_SHL_RIGHT;
                end
                I2S_TX_STATE_SHL_LEFT:
                begin
                    case(WordSelect)
                    WordSelect16bit:
                        if(count == 6'd8) 
                            tx_state <= I2S_TX_STATE_LD_RIGHT;
                        else
                            tx_state <= I2S_TX_STATE_SHL_LEFT;                          
                    WordSelect32bit:
                    case(count)
                        6'd24: tx_state <= (DataBits > 8) ? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_SHL_LEFT;
                        6'd16: tx_state <= I2S_TX_STATE_LD_RIGHT;  
                    default: tx_state <= I2S_TX_STATE_SHL_LEFT; 
                    endcase
                    WordSelect48bit:
                    case(count)
                        6'd40: tx_state <= (DataBits >  8) ? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_SHL_LEFT;
                        6'd32: tx_state <= (DataBits > 16) ? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_SHL_LEFT;
                        6'd24: tx_state <= I2S_TX_STATE_LD_RIGHT;
                    default: tx_state <= I2S_TX_STATE_SHL_LEFT;
                    endcase
                    WordSelect64bit:
                    case(count)
                        56: tx_state <= (DataBits >  8) ? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_SHL_LEFT;
                        48: tx_state <= (DataBits > 16) ? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_SHL_LEFT;
                        40: tx_state <= (DataBits > 24) ? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_SHL_LEFT;              
                        32: tx_state <= I2S_TX_STATE_LD_RIGHT;
                    default: tx_state <= I2S_TX_STATE_SHL_LEFT;
                    endcase 
                    endcase
                end
                I2S_TX_STATE_SHL_RIGHT:
                begin
                    case(WordSelect)
                    WordSelect16bit:
                    if(count == 0)
                        if(txenable) tx_state <= I2S_TX_STATE_LD_LEFT;
                        else tx_state <= I2S_TX_STATE_ZERO;
                    else
                        tx_state <= I2S_TX_STATE_SHL_RIGHT;
                    WordSelect32bit:
                    case(count)            
                        6'd8 : tx_state <= (DataBits > 8)? I2S_TX_STATE_LD_RIGHT : I2S_TX_STATE_SHL_RIGHT;
                        6'd0: tx_state <= (txenable)? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_ZERO;
                    default: tx_state <= I2S_TX_STATE_SHL_RIGHT;
                    endcase
                    WordSelect48bit:
                    case(count)
                        6'd16: tx_state <= (DataBits > 8)? I2S_TX_STATE_LD_RIGHT : I2S_TX_STATE_SHL_RIGHT;         
                        6'd8 : tx_state <= (DataBits > 16)? I2S_TX_STATE_LD_RIGHT : I2S_TX_STATE_SHL_RIGHT;
                        6'd0 : tx_state <= (txenable)? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_ZERO;
                    default: tx_state <= I2S_TX_STATE_SHL_RIGHT;
                    endcase
                    WordSelect64bit:
                    case(count)
                        24: tx_state <= (DataBits > 8)? I2S_TX_STATE_LD_RIGHT : I2S_TX_STATE_SHL_RIGHT;        
                        16: tx_state <= (DataBits > 16)? I2S_TX_STATE_LD_RIGHT : I2S_TX_STATE_SHL_RIGHT;
                        8 : tx_state <= (DataBits > 24)? I2S_TX_STATE_LD_RIGHT : I2S_TX_STATE_SHL_RIGHT;
                        0 : tx_state <= (txenable)? I2S_TX_STATE_LD_LEFT : I2S_TX_STATE_ZERO;
                    default: tx_state <= I2S_TX_STATE_SHL_RIGHT;
                    endcase 
                    endcase
                end
                endcase  
            end 
        end     
        
    /****************************************************************************/
    /*                    Tx Underflow Implementation                           */
    /****************************************************************************/ 
        assign tx_underflow=(TxDataInterleaving) ? 
               ((tx_state == I2S_TX_STATE_LD_LEFT | tx_state == I2S_TX_STATE_LD_RIGHT)& tx_f0_empty_stat):
               (((tx_state == I2S_TX_STATE_LD_LEFT)& tx_f0_empty_stat) |
                ((tx_state == I2S_TX_STATE_LD_RIGHT)& tx_f1_empty_stat));

        wire tx_data_out;
                
        assign SDO = (ChannelResolution == DataBits)? tx_data_out : 
                     (((count <= MsbLeft)&(count >= LsbLeft))|((count <= MsbRight)&(count >= LsbRight))) ? 
                     tx_data_out : 1'b0;                                        
    
        /* Two different datapath configurations are used depend on
        * TxDataInterleaving parameter */
        localparam DP_CONF_INTERLEVED = {
                    `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
                    `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:ZERO */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:SHIFT_LEFT_CHANNEL */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG2 Comment:LOAD_LEFT_CHANNEL */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG3 Comment:LOAD_RIGHT_CHANNEL */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG4 Comment:SHIFT_RIGHT_CHANNEL */
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
                    `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
                    `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
                    `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
                    `SC_SI_A_DEFSI, /*SC_REG6 Comment: */
                    `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
                    1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
                    `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
                    `SC_FB_NOCHN, `SC_CMP1_NOCHN,
                    `SC_CMP0_NOCHN, /*SC_REG7 Comment: */
                     10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
                    `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
                    `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */        
        };
        localparam DP_CONF_SEPARATED = {
                    `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
                    `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:ZERO */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:SHIFT_LEFT_CHANNEL */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG2 Comment:LOAD_LEFT_CHANNEL */
                    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
                    `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___F1,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG3 Comment:LOAD_RIGHT_CHANNEL */
                    `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
                    `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG4 Comment:SHIFT_RIGHT_CHANNEL */
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
                    `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
                    `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
                    `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
                    `SC_SI_A_DEFSI, /*SC_REG6 Comment: */
                    `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
                    1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
                    `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
                    `SC_FB_NOCHN, `SC_CMP1_NOCHN,
                    `SC_CMP0_NOCHN, /*SC_REG7 Comment: */
                     10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
                    `SC_FIFO__EDGE,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
                    `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */        
        };       
        localparam tx_dp_config = (TxDataInterleaving)? DP_CONF_INTERLEVED : DP_CONF_SEPARATED;
        
        /* Tx datapath instantiation */
        cy_psoc3_dp8 #(.cy_dpconfig_a(tx_dp_config))
        dpTx(
        /*  input                   */  .clk(clock),
        /*  input   [02:00]         */  .cs_addr(tx_state),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
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
        /*  output                  */  .so(tx_data_out),
        /*  output                  */  .f0_bus_stat(tx_f0_not_full_stat), /* FIFO 0 is not FULL */
        /*  output                  */  .f0_blk_stat(tx_f0_empty_stat),    /* FIFO 0 is EMPTY */
        /*  output                  */  .f1_bus_stat(tx_f1_not_full_stat), /* FIFO 1 is not FULL */
        /*  output                  */  .f1_blk_stat(tx_f1_empty_stat)     /* FIFO 1 is EMPTY */
        );
    end
    endgenerate    /* Tx */

    /****************************************************************************/
    /*                 Rx direction implementation                              */          
    /****************************************************************************/
    localparam RxDirectionPresent = (Direction==RxPresent || Direction==RxAndTxPresent);
    generate
    if(RxDirectionPresent)
    begin: Rx
    wire rx_clock = ~clock;
    /****************************************************************************/
    /*        Rx Status Register Implementation                                 */
    /****************************************************************************/
        /*    RX Status Register bit location (bits 6-3 unused)    */
        localparam RX_FIFO_OVERFLOW    = 3'd0;
        localparam RX_FIFO_0_NOT_EMPTY = 3'd1;
        localparam RX_FIFO_1_NOT_EMPTY = 3'd2;
        localparam [6:0] RxIntSource = {4'b0, InterruptSource[5:3]};
        wire rx_f0_not_empty_stat;    /* Rx FIFO 0 empty status bit */     
        wire rx_f1_not_empty_stat;    /* Rx FIFO 1 empty status bit */
        wire rx_f0_full_stat;         /* Rx FIFO 0 full status bit  */
        wire rx_f1_full_stat;         /* Rx FIFO 1 full status bit  */
        wire [6:0] rx_status;         /* Rx status register input */
        wire rx_overflow;
        assign rx_status[RX_FIFO_OVERFLOW]    = rx_overflow; 
        assign rx_status[RX_FIFO_0_NOT_EMPTY] = rx_f0_not_empty_stat;
        assign rx_status[RX_FIFO_1_NOT_EMPTY] = rx_f1_not_empty_stat;
        assign rx_status[6:3] = 4'b0;      /* Unused bits of Rx status */
      /* Instantiation of Rx status register*/
        cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'b0000111), 
            .cy_int_mask(RxIntSource)) 
        rx_sts_reg(
        /* input          */  .clock(rx_clock),
        /* input  [06:00] */  .status(rx_status),
        /* output         */  .interrupt(rx_interrupt)      
        );
    /****************************************************************************/
    /*        Rx DMA Request Outputs Implementation                             */
    /****************************************************************************/ 
        assign rx_DMA0 = rx_f0_not_empty_stat;  
        assign rx_DMA1 = rx_f1_not_empty_stat;
        /* RX State Machine States: */
        localparam I2S_RX_STATE_IDLE           = 2'd0;
        localparam I2S_RX_STATE_SHIFT_LEFT     = 2'd1;
        localparam I2S_RX_STATE_LOAD_LEFT      = 2'd2; 
        localparam I2S_RX_STATE_LOAD_RIGHT     = 2'd3;
        reg [1:0] rx_state;
        reg loadLeft, loadRight;
        always @(posedge clock or posedge reset)
        begin
            if(reset) 
            begin
                rx_state <= I2S_RX_STATE_IDLE;
            end
            else
            begin
                case(rx_state)
                I2S_RX_STATE_IDLE: 
                if((count[5:0] == I2SBitCounterPeriod) && (rxenable == 1))
                begin
                    rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                end
                else
                begin
                    rx_state <= I2S_RX_STATE_IDLE;
                end
                I2S_RX_STATE_SHIFT_LEFT:
                begin
                case(WordSelect)
                WordSelect64bit:   
                       case(count)
                        6'd63: rx_state <= (DataBits > 24)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd55: rx_state <= I2S_RX_STATE_LOAD_LEFT;
                        6'd47: rx_state <= (DataBits >  8)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd39: rx_state <= (DataBits > 16)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd31: rx_state <= (DataBits > 24)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd23: rx_state <= I2S_RX_STATE_LOAD_RIGHT;
                        6'd15: rx_state <= (DataBits >  8)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd7 : rx_state <= (DataBits > 16)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd0 : rx_state <= rxenable ? I2S_RX_STATE_SHIFT_LEFT : I2S_RX_STATE_IDLE;
                    default: rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                    endcase
                WordSelect48bit:
                    case(count)
                        6'd47: rx_state <= (DataBits > 16)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd39: rx_state <= I2S_RX_STATE_LOAD_LEFT;
                        6'd31: rx_state <= (DataBits >  8)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd23: rx_state <= (DataBits > 16)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd15: rx_state <= I2S_RX_STATE_LOAD_RIGHT;
                        6'd7 : rx_state <= (DataBits >  8)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd0 : rx_state <= rxenable ? I2S_RX_STATE_SHIFT_LEFT : I2S_RX_STATE_IDLE; 
                    default: rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                    endcase
                WordSelect32bit:
                    case(count)
                        6'd31: rx_state <= (DataBits >  8)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd23: rx_state <= I2S_RX_STATE_LOAD_LEFT;
                        6'd15: rx_state <= (DataBits >  8)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd7 : rx_state <= I2S_RX_STATE_LOAD_RIGHT;
                        6'd0 : rx_state <= rxenable ? I2S_RX_STATE_SHIFT_LEFT : I2S_RX_STATE_IDLE;   
                    default: rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                    endcase
                WordSelect16bit:
                    case(count)
                        6'd15: rx_state <= I2S_RX_STATE_LOAD_RIGHT;
                        6'd7 : rx_state <= I2S_RX_STATE_LOAD_LEFT;
                        6'd0 : rx_state <= rxenable ? I2S_RX_STATE_SHIFT_LEFT : I2S_RX_STATE_IDLE;
                    default: rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                    endcase 
                endcase
                end               
                I2S_RX_STATE_LOAD_LEFT:
                    rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                I2S_RX_STATE_LOAD_RIGHT:
                    rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                default: rx_state <= I2S_RX_STATE_IDLE;
                endcase
            end
        end  

        always @(rx_state)
        begin
            if (rx_state == I2S_RX_STATE_LOAD_LEFT) 
                loadLeft = 1'b1;
            else
                loadLeft = 1'b0;
            if (rx_state == I2S_RX_STATE_LOAD_RIGHT)
                loadRight = 1'b1;    
            else 
                loadRight = 1'b0;
        end 
        wire rx_f0_load = RxDataInterleaving ? loadLeft | loadRight : loadLeft;
        wire rx_f1_load = RxDataInterleaving ? 1'b0 : loadRight;
                    
    /****************************************************************************/
    /*                Rx Overflow Implementation                                       */
    /****************************************************************************/    
        assign rx_overflow = (rx_f0_load & rx_f0_full_stat)|(rx_f1_load & rx_f1_full_stat);
        
        wire rx_data_in; 
        assign rx_data_in = (ChannelResolution == DataBits)? SDI : 
                    (((count <= MsbLeft)&&(count >= LsbLeft))||((count <= MsbRight)&&(count >= LsbRight))) ? SDI:1'b0;  
                 
        /* Rx datapath instantiation */                
                cy_psoc3_dp8 #(.cy_dpconfig_a(
                {
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_A0,
                    `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG0 Comment:RX_IDLE_STATE */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG1 Comment:RX_SHIFT_LEFT */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG2 Comment:RX_FIFO_LOAD */
                    `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
                    `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
                    `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
                    `CS_CMP_SEL_CFGA, /*CS_REG3 Comment:RX_FIFO_LOAD */
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
                    `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
                    `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
                    `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
                    `SC_SI_A_ROUTE, /*SC_REG6 Comment:SI A source MUST BE ROUTED FROM SI INPUT */
                    `SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
                    1'b0, `SC_FIFO1__A0, `SC_FIFO0__A0,
                    `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
                    `SC_FB_NOCHN, `SC_CMP1_NOCHN,
                    `SC_CMP0_NOCHN, /*SC_REG7 Comment:A0 MUST BE FIFO SOURCE */
                     10'h0, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
                    `SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
                    `SC_WRK16CAT_DSBL /*SC_REG8 Comment: */
                })) dpRx(
        /*  input                   */  .clk(rx_clock),
        /*  input   [02:00]         */  .cs_addr({1'b0, rx_state}),
        /*  input                   */  .route_si(rx_data_in),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(rx_f0_load),
        /*  input                   */  .f1_load(rx_f1_load),
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
        /*  output                  */  .so(),
        /*  output                  */  .f0_bus_stat(rx_f0_not_empty_stat), /* FIFO 0 is not EMPTY */
        /*  output                  */  .f0_blk_stat(rx_f0_full_stat),  /* FIFO 0 is FULL */
        /*  output                  */  .f1_bus_stat(rx_f1_not_empty_stat),  /* FIFO 1 is not EMPTY */
        /*  output                  */  .f1_blk_stat(rx_f1_full_stat)   /* FIFO 1 is FULL */
        );        
    end
    
    endgenerate /* Rx */
    
/*`#end` -- edit above this line, do not edit this line */
endmodule

`endif /*I2S_V_v1_0_ALREADY_INCLUDED*/

