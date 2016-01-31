/*******************************************************************************
* File Name: I2S_v2_0.v
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
*  +=====+-------+-------+-------+--------+--------+-------+--------+--------+
*  | Bit |   7   |   6   |   5   |   4    |   3    |   2   |   1    |   0    |
*  +=====+-------+-------+-------+--------+--------+-------+--------+--------+
*  |Desc |        unused         |        |        |enable |rxenable|txenable| 
*  +=====+-------+-------+-------+--------+--------+-------+--------+--------+
*
*    txenable     =>   0 = disable Tx Direction
*                      1 = enable Tx Direction
*                      
*    rxenable     =>   0 = disable Rx Direction
*                      1 = enable Rx Direction
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
* A1 => not used
*
********************************************************************************
* I*O Signals:
********************************************************************************
*    name              direction       Description
*    sck               output          I2S clock line                      
*    sdo               output          I2S output data line                
*    ws                output          I2S word select line                
*    sdi               input           I2S input data line                 
*    clock             input           System clock                        
*    rx_dma0           output          Rx direction DMA request for FIFO 0 
*    rx_dma1           output          Rx direction DMA request for FIFO 1 
*    rx_interrupt      output          Rx direction interrupt              
*    tx_dma0           output          Tx direction DMA request for FIFO 0 
*    tx_dma1           output          Tx direction DMA request for FIFO 1 
*    tx_interrupt      output          Tx direction interrupt              
*
********************************************************************************
* Copyright 2008-2010, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef I2S_v2_0_V_ALREADY_INCLUDED
`else
`define I2S_v2_0_V_ALREADY_INCLUDED

module I2S_v2_0 (
    output  wire    sck,             /* I2S clock line                      */
    output  wire    sdo,             /* I2S output data line                */
    output  wire    ws,              /* I2S word select line                */
    input   wire    sdi,             /* I2S input data line                 */
    input   wire    clock,           /* System clock                        */
    output  wire    rx_dma0,         /* Rx direction DMA request for FIFO 0 */
    output  wire    rx_dma1,         /* Rx direction DMA request for FIFO 1 */
    output  wire    rx_interrupt,    /* Rx direction interrupt              */
    output  wire    tx_dma0,         /* Tx direction DMA request for FIFO 0 */
    output  wire    tx_dma1,         /* Tx direction DMA request for FIFO 1 */
    output  wire    tx_interrupt     /* Tx direction interrupt              */
);
    
    /***************************************************************************
    *              Parameters                                                
    ***************************************************************************/
    parameter  [5:0] DataBits           = 6'd16;
    parameter  [6:0] WordSelect         = 7'd32;
    parameter  [1:0] Direction          = 2'd3;
    parameter  [5:0] InterruptSource    = 6'h00;
    parameter  RxDataInterleaving       = 1;
    parameter  RxDMA_present            = 0;
    parameter  TxDataInterleaving       = 1;
    parameter  TxDMA_present            = 0;
    
    localparam [5:0] ChannelResolution   = WordSelect/2; 
    localparam [5:0] I2SBitCounterPeriod = WordSelect-1;               
    
    /***************************************************************************
    * MSB and LSB position definition. MSB and LSB are used to determine unused
    * bits for each direction. Any unused bits will be ignored on Tx and will 
    * 0 on Rx.
    ***************************************************************************/

    localparam [5:0] MsbLeft = I2SBitCounterPeriod - 1;
    localparam [5:0] LsbLeft = I2SBitCounterPeriod - DataBits;
    
    localparam [5:0] MsbRight = MsbLeft - ChannelResolution;
    localparam [5:0] LsbRight = LsbLeft - ChannelResolution;
    
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
    *       Control Register Implementation                                      
    ***************************************************************************/   
    wire [7:0] ctrl;
    /* Control Register bit location (bits 3-7 are unused) */
    localparam I2S_CTRL_TX_ENABLE       = 3'd0;
    localparam I2S_CTRL_RX_ENABLE       = 3'd1;
    localparam I2S_CTRL_ENABLE          = 3'd2;
    
    /* Add support of sync mode for PSoC3 ES3 Rev */
    generate
    if(PSOC3_ES2 || PSOC5_ES1)
    begin: AsyncCtl
        cy_psoc3_control #(.cy_force_order(1)) ControlReg
        (
            /* output [07:00] */  .control(ctrl)
        );
    end /* AsyncCtl */
    else
    begin: SyncCtl
        /* The clock to operate Control Reg for ES3 must be synchronous and run
        *  continuosly. In this case the udb_clock_enable is used only for 
        *  synchronization. The resulted clock is always enabled.
        */ 
        wire ctl_clk; /* clock to operate Control Reg */
        
        cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) CtlClkSync
        (
            /* input  */    .clock_in(clock),
            /* input  */    .enable(1'b1),
            /* output */    .clock_out(ctl_clk)
        );  
        cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'h07)) ControlReg
        (
            /*  input         */  .clock(ctl_clk),
            /* output [07:00] */  .control(ctrl)
        );
    end /* SyncCtl */
    endgenerate
    
    wire enable = ctrl[I2S_CTRL_ENABLE];           /* enable I2S */
        
    /***************************************************************************
    *         Instantiation of udb_clock_enable  
    ****************************************************************************
    * The udb_clock_enable primitive component allows to support clock enable 
    * mechanism and specify the intended synchronization behaviour for the clock 
    * result (operational clock).
    */
    wire op_clk;    /* operational clock */
    
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(enable),
        /* output */    .clock_out(op_clk)
    );                         
         
    /***************************************************************************
    *       7-bit Down Counter implementation                                      
    ***************************************************************************/
    wire [6:0]  count; 
    cy_psoc3_count7 #(.cy_period({I2SBitCounterPeriod,1'b1})) BitCounter
    (
        /*  input             */  .clock(op_clk),
        /*  input             */  .reset(1'b0),
        /*  input             */  .load(1'b0),
        /*  input             */  .enable(1'b1),
        /*  output  [06:00]   */  .count(count),
        /*  output            */  .tc()
    );
    
    assign sck = ~count[0] & enable;    /* I2S output clock generation  */
    wire channel;
    localparam WORD_SEL16     = 7'd16;
    localparam WORD_SEL32     = 7'd32;
    localparam WORD_SEL48     = 7'd48;
    localparam WORD_SEL64     = 7'd64;

    /***************************************************************************
    * The channel bit is used to distinguish LEFT or RIGHT channel will be 
    * loaded. Then in conjuction with TxDataInterleaving parameter value it
    * defines which FIFO is source for each channel. 
    ***************************************************************************/   
    generate  /* Channel Select generation */
    if(WordSelect == WORD_SEL16)
    begin: WS16
        assign channel = ~count[4];
    end
    else
    begin: WS32
        if (WordSelect == WORD_SEL32)
        begin
            assign channel = ~count[5];
        end
        else
        begin: WS48
            if (WordSelect == WORD_SEL48)
            begin
                assign channel = ~(count[6] | count[5] & count[4]);                    
            end
            else
            begin: WS64
                assign channel = ~count[6];
            end
        end
    end
    endgenerate  /* Channel Select generation */
    
    assign ws = channel & enable;  /* Word Select Output Generation */
    
    /*************************************************************************** 
    * If channel resolution is less than half of Word Select period all unused 
    * bits on Rx and Tx directions must be truncated by zero                   
    ***************************************************************************/
    wire data_trunc = (ChannelResolution == DataBits)? 1'b0 : 
                (((count[6:1] <= MsbLeft) & (count[6:1] >= LsbLeft))|
                 ((count[6:1] <= MsbRight) & (count[6:1] >= LsbRight))) ? 1'b0 : 1'b1;
    
    localparam RX_PRESENT          = 2'd1;
    localparam TX_PRESENT          = 2'd2;
    localparam RX_AND_TX_PRESENT   = 2'd3;
    
    /***************************************************************************
    *                      TX direction implementation                         
    ***************************************************************************/
    localparam TxDirectionPresent = (Direction == TX_PRESENT || Direction == RX_AND_TX_PRESENT); 
    generate
    if(TxDirectionPresent)
    begin: Tx
        /***********************************************************************
        *        Tx Status Register Implementation                           
        ***********************************************************************/
        /*    TX Status Register bit location (bits 6-3 unused)    */
        localparam TX_FIFO_UNDERFLOW  = 3'd0;
        localparam TX_FIFO_0_NOT_FULL = 3'd1;
        localparam TX_FIFO_1_NOT_FULL = 3'd2;
        localparam [6:0] TxIntSource = {4'b0, InterruptSource[2:0]}; 
        wire tx_f0_not_full_stat;           /* Tx FIFO 0 not full status bit */ 
        wire tx_f1_not_full_stat;           /* Tx FIFO 1 not full status bit */
        wire tx_f0_empty_stat;              /* Tx FIFO 0 empty status bit    */
        wire tx_f1_empty_stat;              /* Tx FIFO 1 empty status bit    */
        wire [6:0] tx_status;               /* status register input         */    
        wire tx_underflow;
        assign tx_status[TX_FIFO_UNDERFLOW]  = tx_underflow;
        assign tx_status[TX_FIFO_0_NOT_FULL] = tx_f0_not_full_stat;
        assign tx_status[TX_FIFO_1_NOT_FULL] = (TxDataInterleaving)? 1'b0 : tx_f1_not_full_stat;
        assign tx_status[6:3] = 4'b0;        /* Status register unused bits */ 
        
        /* Instantiate the status register */ 
        cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h01), .cy_int_mask(TxIntSource)) TxStsReg
        (
            /* input          */  .clock(op_clk),
            /* input  [06:00] */  .status(tx_status),
            /* output         */  .interrupt(tx_interrupt)  
        );
   
        /***********************************************************************
        *        Tx DMA Request Outputs Implementation                       
        ***********************************************************************/   
        assign tx_dma0 = tx_f0_not_full_stat;
        assign tx_dma1 = tx_f1_not_full_stat;
        
        /***********************************************************************
        * The transition into and out of the enabled state will occur at a word 
        * select boundary such that a left / right sample pair is always 
        * transmitted. So register the enable signal at the end of counter down.
        ***********************************************************************/
        reg txenable; 
        always @(posedge op_clk)
        begin
        if(count[6:0] == 7'd0)
            txenable <= ctrl[I2S_CTRL_TX_ENABLE];
        end
        
        /* TX State Machine States: */
        localparam I2S_TX_STATE_HOLD      = 3'd0;
        localparam I2S_TX_STATE_SHIFT     = 3'd1;
        localparam I2S_TX_STATE_LOAD      = 3'd2;
        localparam I2S_TX_STATE_ZERO      = 3'd3;

        reg [1:0] tx_state;
        always @(posedge op_clk)
        begin
            if(txenable)
            begin
                if(count[0] == 1'b1)
                begin 
                case(WordSelect)
                    WORD_SEL64: 
                    case(count[6:1])
                        6'd63: tx_state <= I2S_TX_STATE_LOAD;
                        6'd55: tx_state <= (DataBits > 8)  ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                        6'd47: tx_state <= (DataBits > 16) ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;              
                        6'd39: tx_state <= (DataBits > 24) ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                        6'd31: tx_state <= I2S_TX_STATE_LOAD;
                        6'd23: tx_state <= (DataBits > 8)  ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                        6'd15: tx_state <= (DataBits > 16) ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;              
                        6'd7 : tx_state <= (DataBits > 24) ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                    default: tx_state <= I2S_TX_STATE_SHIFT;
                    endcase
                    WORD_SEL48: 
                    case(count[6:1])
                        6'd47: tx_state <= I2S_TX_STATE_LOAD;              
                        6'd39: tx_state <= (DataBits > 8)  ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                        6'd31: tx_state <= (DataBits > 16) ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                        6'd23: tx_state <= I2S_TX_STATE_LOAD;
                        6'd15: tx_state <= (DataBits > 8)  ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;              
                        6'd7 : tx_state <= (DataBits > 16) ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                    default: tx_state <= I2S_TX_STATE_SHIFT;
                    endcase         
                    WORD_SEL32:  
                    case(count[6:1])
                        6'd31: tx_state <= I2S_TX_STATE_LOAD;
                        6'd23: tx_state <= (DataBits > 8) ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                        6'd15: tx_state <= I2S_TX_STATE_LOAD;
                        6'd7 : tx_state <= (DataBits > 8) ? I2S_TX_STATE_LOAD : I2S_TX_STATE_SHIFT;
                    default: tx_state <= I2S_TX_STATE_SHIFT;
                    endcase
                    WORD_SEL16:
                    case(count[6:1])
                        6'd15: tx_state <= I2S_TX_STATE_LOAD;
                        6'd7 : tx_state <= I2S_TX_STATE_LOAD;
                    default: tx_state <= I2S_TX_STATE_SHIFT;
                    endcase
                endcase    
                end  
                else
                begin
                    tx_state <= I2S_TX_STATE_HOLD;
                end
            end
            else
            begin
                tx_state <= I2S_TX_STATE_ZERO;
            end
        end    /* Tx protocol state machine */     
                    
        /***********************************************************************
        *                    Tx Underflow Implementation                        
        ***********************************************************************/ 
        assign tx_underflow = (TxDataInterleaving) ? 
               (tx_state == I2S_TX_STATE_LOAD & tx_f0_empty_stat):
               (tx_state == I2S_TX_STATE_LOAD)& ((tx_f0_empty_stat & ~channel) | (tx_f1_empty_stat & channel));
        
        /* Determine which cannel is transmitted. Register this signal because it 
        * is driven directly to Tx datapath control (cs_addr) */
        reg tx_channel;     
        always @(posedge op_clk)
        begin
            tx_channel <= channel;
        end
        
        wire tx_data_out;
        assign sdo = (data_trunc)? 1'b0 : tx_data_out;   /* truncate unused bits */       
        wire [2:0] tx_dp_addr = (TxDataInterleaving)? {1'b0, tx_state} : {tx_channel, tx_state}; 
        
        /* Tx datapath instantiation */
        cy_psoc3_dp8 #(.cy_dpconfig_a(
        {
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG0 Comment:HOLD STATE */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG1 Comment:SHIFT_LEFT_CHANNEL */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:LOAD_LEFT_CHANNEL */
        	`CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:ZERO */
        	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:IDLE*/
        	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        	`CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:SHIFT_RIGHT_CHANNEL */
        	`CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___F1,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:LOAD_RIGHT_CHANNEL */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment: IDLE*/
        	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
        	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
        	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        	`SC_SI_A_DEFSI, /*SC_REG6 Comment: */
        	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
        	1'b0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
        	`SC_CMP0_NOCHN, /*SC_REG7 Comment: */
            3'h0, `SC_FIFO_SYNC__ADD, 6'h0,    
            `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,   
        	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: re-sample fx_blk_stat to DP clock */
        })) dpTx(
            /*  input                   */  .clk(op_clk),
            /*  input   [02:00]         */  .cs_addr(tx_dp_addr),
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

    /***************************************************************************
    *                 Rx direction implementation                                        
    ***************************************************************************/
    localparam RxDirectionPresent = (Direction == RX_PRESENT || Direction == RX_AND_TX_PRESENT);
    generate
    if(RxDirectionPresent)
    begin: Rx
        /***********************************************************************
        *        Rx Status Register Implementation                              
        ***********************************************************************/
        /*    RX Status Register bit location (bits 6-3 unused)    */
        localparam RX_FIFO_OVERFLOW    = 3'd0;
        localparam RX_FIFO_0_NOT_EMPTY = 3'd1;
        localparam RX_FIFO_1_NOT_EMPTY = 3'd2;
        localparam [6:0] RxIntSource = {4'b0, InterruptSource[5:3]};
        
        wire rx_f0_not_empty_stat;    /* Rx FIFO 0 empty status bit */     
        wire rx_f1_not_empty_stat;    /* Rx FIFO 1 empty status bit */
        wire rx_f0_full_stat;         /* Rx FIFO 0 full status bit  */
        wire rx_f1_full_stat;         /* Rx FIFO 1 full status bit  */
        wire [6:0] rx_status;         /* Rx status register input   */
        wire rx_overflow;
        assign rx_status[RX_FIFO_OVERFLOW]    = rx_overflow; 
        assign rx_status[RX_FIFO_0_NOT_EMPTY] = rx_f0_not_empty_stat;
        assign rx_status[RX_FIFO_1_NOT_EMPTY] = (RxDataInterleaving)? 1'b0 : rx_f1_not_empty_stat;
        assign rx_status[6:3] = 4'b0;      /* Unused bits of Rx status */
        
        /* Instantiation of Rx status register*/
        cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h01), .cy_int_mask(RxIntSource)) RxStsReg
        (
            /* input          */  .clock(op_clk),
            /* input  [06:00] */  .status(rx_status),
            /* output         */  .interrupt(rx_interrupt)      
        );
        /***********************************************************************
        *        Rx DMA Request Outputs Implementation                         
        ***********************************************************************/ 
        assign rx_dma0 = rx_f0_not_empty_stat;  
        assign rx_dma1 = rx_f1_not_empty_stat;
        
        /***********************************************************************
        * The transition into and out of the enabled state will occur at a word 
        * select boundary such that a left / right sample pair is always received.
        * Since the counter period value is used to capture the least signifficant
        * data byte of right channel, there is one unwanted (false) load, because 
        * of rx enabling on the counter period value. To avoid the load the enabling
        * of the Rx is delayed by 1.
        ***********************************************************************/
        reg rxenable; 
        always @(posedge op_clk)
        begin
        if(count[6:1] == (I2SBitCounterPeriod-1))
            rxenable <= ctrl[I2S_CTRL_RX_ENABLE];
        end   
                 
        /* RX State Machine States: */
        localparam I2S_RX_STATE_HOLD           = 3'd0;
        localparam I2S_RX_STATE_SHIFT_LEFT     = 3'd1;
        localparam I2S_RX_STATE_LOAD_LEFT      = 3'd3;
        localparam I2S_RX_STATE_LOAD_RIGHT     = RxDataInterleaving ? 3'd3 : 3'd5;
        
        reg [2:0] rx_state;
        
        always @(posedge op_clk)
        begin
            if(rxenable) 
            begin
                if(count[0] == 1'b0)
                begin
                    case(WordSelect)
                    WORD_SEL64:  
                        case(count[6:1])
                        6'd63: rx_state <= (DataBits > 24)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd55: rx_state <= I2S_RX_STATE_LOAD_LEFT;
                        6'd47: rx_state <= (DataBits > 8)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd39: rx_state <= (DataBits > 16)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd31: rx_state <= (DataBits > 24)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd23: rx_state <= I2S_RX_STATE_LOAD_RIGHT;
                        6'd15: rx_state <= (DataBits > 8)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        6'd7 : rx_state <= (DataBits > 16)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                        default: rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                        endcase
                    WORD_SEL48:
                        case(count[6:1])
                            6'd47: rx_state <= (DataBits > 16)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                            6'd39: rx_state <= I2S_RX_STATE_LOAD_LEFT;
                            6'd31: rx_state <= (DataBits > 8)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                            6'd23: rx_state <= (DataBits > 16)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                            6'd15: rx_state <= I2S_RX_STATE_LOAD_RIGHT;
                            6'd7 : rx_state <= (DataBits > 8)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT; 
                        default: rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                        endcase
                    WORD_SEL32:
                        case(count[6:1])
                            6'd31: rx_state <= (DataBits > 8)? I2S_RX_STATE_LOAD_RIGHT : I2S_RX_STATE_SHIFT_LEFT;
                            6'd23: rx_state <= I2S_RX_STATE_LOAD_LEFT;
                            6'd15: rx_state <= (DataBits > 8)? I2S_RX_STATE_LOAD_LEFT : I2S_RX_STATE_SHIFT_LEFT;
                            6'd7 : rx_state <= I2S_RX_STATE_LOAD_RIGHT;   
                        default: rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                        endcase
                    WORD_SEL16:
                        case(count[6:1])
                            6'd15: rx_state <= I2S_RX_STATE_LOAD_RIGHT;
                            6'd7 : rx_state <= I2S_RX_STATE_LOAD_LEFT;
                        default: rx_state <= I2S_RX_STATE_SHIFT_LEFT;
                        endcase 
                    endcase
                end
                else
                begin
                    rx_state <= I2S_RX_STATE_HOLD;
                end
            end
            else
            /******************************************************************* 
            * Because of the delay for the Rx enabling the first bit should be 
            * captured in preparation for the Rx being enabled. So rx has to 
            * shift input data continuously, even if it isn't enabled 
            *******************************************************************/ 
            begin
                rx_state <= I2S_RX_STATE_SHIFT_LEFT;
            end
        end   /* Rx protocol state machine */
                             
        wire rx_f0_load = rx_state[1];
        wire rx_f1_load = rx_state[2];
                    
        /***********************************************************************
        *                Rx Overflow Implementation                            
        ***********************************************************************/    
        assign rx_overflow = (RxDataInterleaving)? (rx_f0_load & rx_f0_full_stat):
                             ((rx_f0_load & rx_f0_full_stat)|(rx_f1_load & rx_f1_full_stat));     
                             
        wire rx_data_in = (data_trunc)? 1'b0 : sdi;  /* truncate unused bits */
                 
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
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG2 Comment:RX_IDLE_STATE */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG3 Comment:RX_IDLE_STATE */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG4 Comment:RX_IDLE_STATE */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG5 Comment:RX_IDLE_STATE */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG6 Comment:RX_IDLE_STATE */
        	`CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        	`CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        	`CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        	`CS_CMP_SEL_CFGA, /*CS_REG7 Comment:RX_IDLE_STATE */
        	  8'hFF, 8'h00,	/*SC_REG4	Comment: */
        	  8'hFF, 8'hFF,	/*SC_REG5	Comment: */
        	`SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        	`SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        	`SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        	`SC_SI_A_ROUTE, /*SC_REG6 Comment:SI A source must be routed from SI input */
        	`SC_A0_SRC_ACC, `SC_SHIFT_SL, 1'b0,
        	1'b0, `SC_FIFO1__A0, `SC_FIFO0__A0,
        	`SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        	`SC_FB_NOCHN, `SC_CMP1_NOCHN,
        	`SC_CMP0_NOCHN, /*SC_REG7 Comment:FIFO source is A0 */
            3'h0, `SC_FIFO_SYNC__ADD, 6'h0,    
            `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,   
        	`SC_FIFO_LEVEL,`SC_FIFO__SYNC,`SC_EXTCRC_DSBL,
        	`SC_WRK16CAT_DSBL /*SC_REG8 Comment: re-sample fx_blk_stat to DP clock */
        })) dpRx(
            /*  input                   */  .clk(op_clk),
            /*  input   [02:00]         */  .cs_addr({2'b0, rx_state[0]}),
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
            /*  output                  */  .f0_blk_stat(rx_f0_full_stat),      /* FIFO 0 is FULL */
            /*  output                  */  .f1_bus_stat(rx_f1_not_empty_stat), /* FIFO 1 is not EMPTY */
            /*  output                  */  .f1_blk_stat(rx_f1_full_stat)       /* FIFO 1 is FULL */
        );        
    end    
    endgenerate /* Rx */

endmodule

`endif /*I2S_v2_0_V_ALREADY_INCLUDED*/

