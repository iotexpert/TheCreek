/*******************************************************************************
* File Name: bSPDIF_Tx_v1_10.v
* Version `$CY_MAJOR_VERSION`.`$CY_MINOR_VERSION`
*
* Description:
* The SPDIF_Tx module is Sony Philips digital audio transmitter interface 
* implementation done with UDB's. It supports up to 24 bits per sample and
* up to 192 kHz sample rate. 
*
*------------------------------------------------------------------------------
*                 Control and Status Register definitions
*------------------------------------------------------------------------------
*
*  Control Register Definition
*  +=====+-------+-------+-------+--------+--------+-------+--------+--------+
*  | Bit |   7   |   6   |   5   |   4    |   3    |   2   |   1    |   0    |
*  +=====+-------+-------+-------+--------+--------+-------+--------+--------+
*  |Desc |                  UNUSED                 |  RST  |  ENBL  | TXENBL | 
*  +=====+-------+-------+-------+--------+--------+-------+--------+--------+
*
*    TXENBL       =>   0 = disable Audio Transmission
*                      1 = enable Audio Transmission
*                      
*    ENBL         =>   0 = disable SPDIF component
*                      1 = enable Transmission of Preamble and Channel Status
*
*    RST          =>   0 = reset is not active
*                      1 = force the component to the init state 
*    
*  Statusi Register Definition
*  +=======+-----+------+--------+--------+--------+--------+--------+--------+
*  |  Bit  |  7  |  6   |    5   |    4   |    3   |    2   |    1   |   0    |
*  +=======+-----+------+--------+--------+--------+--------+--------+--------+
*  | Desc  | INT | RSVD | CST_F1 | CST_F0 |  CST   |  TX_F1 |  TX_F0 |   TX   |
*  |       |     |      |NOT_FULL|NOT_FULL| UNDFLW |NOT_FULL|NOT_FULL| UNDFLW |
*  +=======+-----+------+--------+--------+--------+--------+--------+--------+
*
*    CST_F1_NOT_FULL  =>  0 = Channel Status FIFO 1 is full 
*                         1 = Channel Status FIFO 1 is not full
*
*    CST_F0_NOT_FULL  =>  0 = Channel Status FIFO 0 is full 
*                         1 = Channel Status FIFO 0 is not full
*
*    CST_UNDFLW       =>  0 = Channel status FIFO underflow event has not occured
*                         1 = Channel status FIFO underflow event has occured
*
*    TX_F1_NOT_FULL   =>  0 = Audio FIFO 1 is full 
*                         1 = Audio FIFO 1 is not full
*
*    TX_F0_NOT_FULL   =>  0 = Audio FIFO 0 is full 
*                         1 = Audio FIFO 0 is not full
*
*    TX_UNDFLW        =>  0 = Audio FIFO underflow event has not occured
*                         1 = Audio FIFO underflow event has occured
*
********************************************************************************
* Data Path register definitions
********************************************************************************
* SPDIF_Tx: FrameCounter
* DESCRIPTION: FrameCounter is used to implement S/PDIF framing.
* REGISTER USAGE:
* F0 => PRE/POST data period in clocks
* F1 => S/PDIF block size in frames
* D0 => Audio data period in clocks
* D1 => constant zero
* A0 => used to count for D0 and F0 sources
* A1 => used to count for F1 source
*
********************************************************************************
* SPDIF_Tx: PreambleGen
* DESCRIPTION:
* REGISTER USAGE: PreambleGen is used to implement S/PDIF preamble generator.  
* F0 => Preamble Z pattern [11101000]
* F1 => not used
* D0 => Preamble X pattern [11100010]
* D1 => Preamble Y pattern [11100100]
* A0 => used to serialize data for F0 and D0 sources
* A1 => used to serialize data for D0 source
*
********************************************************************************
* SPDIF_Tx: AudioTx
* DESCRIPTION: AudioTx is used to implement the interface for audio data
* REGISTER USAGE:
* F0 => Channel 0 Audio FIFO
* F1 => Channel 1 Audio FIFO
* D0 => not used
* D1 => not used
* A0 => used to serialize data for F0 source
* A1 => used to serialize data for F1 source
*
********************************************************************************
* SPDIF_Tx: ChStatusFIFO
* DESCRIPTION:
* REGISTER USAGE: ChStatusFIFO is used to implement the interface for status data 
* F0 => Channel 0 Status FIFO
* F1 => Channel 1 Status FIFO
* D0 => not used
* D1 => not used
* A0 => used to serialize data for F0 source
* A1 => used to serialize data for F1 source
*
********************************************************************************
* I*O Signals:
********************************************************************************
*    name              direction       Description
*    clock             input           Input clock. Must be 2X of bitrate needed                      
*    spdif             output          The data out line                
*    tx_dma0           output          DMA request for Ch0 audio data                
*    tx_dma1           output          DMA request for Ch1 audio data                 
*    cst_dma0          output          DMA request for Ch0 status data                        
*    cst_dma1          output          DMA request for Ch1 status data 
*    interrupt         output          Interrupt line for errors 
*
********************************************************************************
* Copyright 2011-2012, Cypress Semiconductor Corporation.  All rights reserved.
* You may use this file only in accordance with the license, terms, conditions, 
* disclaimers, and limitations in the end user license agreement accompanying 
* the software package with which this file was provided.
********************************************************************************/


`include "cypress.v"
`ifdef bSPDIF_Tx_v1_10_V_ALREADY_INCLUDED
`else
`define bSPDIF_Tx_v1_10_V_ALREADY_INCLUDED

module bSPDIF_Tx_v1_10 (
    output  wire    spdif,      /* The data out line                */
    output  wire    tx_dma0,    /* DMA request for ChA audio data   */
    output  wire    tx_dma1,    /* DMA request for ChB audio data   */
    output  wire    cst_dma0,   /* DMA request for ChA status data  */
    output  wire    cst_dma1,   /* DMA request for ChB status data  */
    output  wire    interrupt,  /* Interrupt line for errors        */ 
    input   wire    clock       /* The component clock              */
);
    
    /***************************************************************************
    *                       Parameters
    ***************************************************************************/
    parameter [4:0] DataBits         = 5'd24; /* The audio data length */
    parameter       InterleavedFifo  = 1'b1;  /* The data interleaving setting */ 

    /***************************************************************************
    *            UDB array version definitions 
    ***************************************************************************/   
    
    localparam CY_UDB_V0 = (`CYDEV_CHIP_MEMBER_USED == `CYDEV_CHIP_MEMBER_5A);    
    localparam CY_UDB_V1 = (!CY_UDB_V0);

    /***************************************************************************
    *         Instantiation of udb_clock_enable primitive 
    ****************************************************************************
    * The udb_clock_enable primitive component allows to support clock enable 
    * mechanism and specify the intended synchronization behavior for the clock 
    * result (operational clock).
    */
    cy_psoc3_udb_clock_enable_v1_0 #(.sync_mode(`TRUE)) ClkSync
    (
        /* input  */    .clock_in(clock),
        /* input  */    .enable(1'b1),
        /* output */    .clock_out(op_clk)
    );         
    
    /***************************************************************************
    *       Control Register Implementation                                      
    ***************************************************************************/   
    wire [7:0] ctrl;
    /* Control Register bit location (bits 2-7 are unused) */
    localparam [2:0] SPDIF_TX_CTRL_TX_ENABLE  = 3'd0;
    localparam [2:0] SPDIF_TX_CTRL_ENABLE     = 3'd1;
        
    /* Control register mode */
    generate
    if(CY_UDB_V0)
    begin: AsyncCtrl
        cy_psoc3_control #(.cy_force_order(1)) ControlReg
        (
            /* output [07:00] */  .control(ctrl)
        );        
    end /* AsyncCtl */
    else
    begin: SyncCtrl         
        cy_psoc3_control #(.cy_force_order(1), .cy_ctrl_mode_1(8'h00), .cy_ctrl_mode_0(8'h03)) ControlReg
        (
            /*  input         */  .clock(op_clk),
            /* output [07:00] */  .control(ctrl)
        );
    end /* SyncCtl */
    endgenerate
        
    /* Reset implementation */
    reg reset;    
    always @(posedge op_clk)
    begin
        reset <= ~ctrl[SPDIF_TX_CTRL_ENABLE];
    end
        
    /***************************************************************************
    *                   Status Register Implementation                           
    ***************************************************************************/
    /* Status Register bit location (bit 6 unused) */
    localparam [2:0] SPDIFTX_AUDIO_FIFO_UNDERFLOW  = 3'd0;
    localparam [2:0] SPDIFTX_AUDIO_FIFO_0_NOT_FULL = 3'd1;
    localparam [2:0] SPDIFTX_AUDIO_FIFO_1_NOT_FULL = 3'd2;
    localparam [2:0] SPDIFTX_CHST_FIFO_UNDERFLOW   = 3'd3;
    localparam [2:0] SPDIFTX_CHST_0_FIFO_NOT_FULL  = 3'd4;
    localparam [2:0] SPDIFTX_CHST_1_FIFO_NOT_FULL  = 3'd5;
    
    /* The inturrupt is generated only for error condition, so set the interrupt
    *  mask only for bit 0 and bit 3.
    */
    localparam [6:0] SPDIFTX_INTERRUPT_SOURCE = 7'h09; 
  
    wire tx_fifo0_not_full;         /* audio FIFO 0 is not full  */
    wire tx_fifo1_not_full;         /* audio FIFO 1 is not full  */
    wire tx_fifo0_empty;            /* audio FIFO 0 is empty     */
    wire tx_fifo1_empty;            /* audio FIFO 1 is emty      */
    wire cst_fifo0_not_full;        /* status FIFO 0 is not full */
    wire cst_fifo1_not_full;        /* status FIFO 1 is not full */ 
    wire cst_fifo0_empty;           /* status FIFO 0 is empty    */
    wire cst_fifo1_empty;           /* status FIFO 1 is emty     */
    
    wire [6:0] status;              /* status register input     */    
    wire tx_underflow;
    wire cst_underflow;
    /* Internal error capturing */
    reg tx_underflow_sticky;
    reg cst_underflow_sticky;
    
    assign status[SPDIFTX_AUDIO_FIFO_UNDERFLOW]  = tx_underflow;
    assign status[SPDIFTX_AUDIO_FIFO_0_NOT_FULL] = tx_fifo0_not_full;
    assign status[SPDIFTX_AUDIO_FIFO_1_NOT_FULL] = (InterleavedFifo)? 1'b0 : tx_fifo1_not_full;
    assign status[SPDIFTX_CHST_FIFO_UNDERFLOW]   = cst_underflow;
    assign status[SPDIFTX_CHST_0_FIFO_NOT_FULL]  = cst_fifo0_not_full;
    assign status[SPDIFTX_CHST_1_FIFO_NOT_FULL]  = cst_fifo1_not_full;
    
    assign status[6] = 1'b0;        /* Status register unused bits */ 
        
    /* Instantiate the status register */ 
    cy_psoc3_statusi #(.cy_force_order(1), .cy_md_select(7'h09), .cy_int_mask(SPDIFTX_INTERRUPT_SOURCE)) StatusReg
    (
        /* input          */  .clock(op_clk),
        /* input  [06:00] */  .status(status),
        /* output         */  .interrupt(interrupt)  
    );
    
    /***************************************************************************
    *                   DMA Request Outputs
    ****************************************************************************
    * The component will request more audio data when the auidio data datapath 
    * FIFOs are not full and more channel status data when channel status 
    * datapath FIFOs are not full. In both cases the bus status signals of 
    * corresponding FIFOs are used.
    */     
    assign tx_dma0  = tx_fifo0_not_full;
    assign tx_dma1  = tx_fifo1_not_full;
    assign cst_dma0 = cst_fifo0_not_full;
    assign cst_dma1 = cst_fifo1_not_full;
    
    /***************************************************************************
    *                   Frame counter implementation
    ***************************************************************************/
    wire fcz0;      /* Frame counter A0 register == 0           */
    wire fcz1;      /* Frame counter A1 register == 0           */
    wire fcce1;     /* Frame counter A1 & 8'h07  == 0. The signal is high every
                       eight frames. It is used to determine when next channel
                       status byte is loaded to the status FIFOs */
    
    /***************************************************************************
    * Frame counter state machine. The state machine counts the number of 
    * frames in SPDIF block (count to 192) and all intervals within a single
    * frame. 
    * Moves to the next state as each counter counts down to 0 whithin a frame
    * interval. Counts the number of frames on Y frame (PREY). 
    * Reset state is at the beginning of the Preamble Z period (PREZ).
    *
    * F0 - The period of preamble (PRE) and post data (POST) counters. Both 
    *      regions are 4 bit width. (set 2 less than twice of period because 
    *      of 2x clocking).
    * D0 - Audio data period counter. The data interval is always 24 bits. 
    *      (set 2 less than twice of period) because of 2x clocking).
    * F1 - The number of frames in SPDIF block. (set to period value).
    * D1 - Constant zero.
    * A0 used to count for D0 and F0 sources.
    * A1 used to count for D1 source.
    * 
    * State Machine has a fixed progression through the states as the counters
    * complete:
    *   PREX (PREZ) - Preamble X(Z) 
    *   DATA_CHA    - Audio data of Channel A
    *   POST_CHA    - Post data of Channel A 
    *   PREY        - Preamble Y (The frame counting is doing here) 
    *   DATA_CHB    - Audio data of Channel B
    *   POST_CHB    - Post data of Channel B 
    *
    * The state machine has 13 states that results in 4 bits for state encoding.
    * But the states are mapped to use the same datapath addresses. This allows
    * using of 3 lower bits of the state machine to directly drive the cs_addr
    * bits. 
    * The most significant bit (bit 3) is '0' for ChA sound and status data 
    * transmission and '1' for ChB. This allow to use this bit for TX and Status
    * datapathes control. 
    ***************************************************************************/

    localparam [3:0] SPDIFTX_FRAMECNT_STATE_PREZ_LD         = 4'h0;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_PREX_LD         = 4'h4;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_PREX_CNT        = 4'h1;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_DATA_CHA_LD     = 4'h3;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_DATA_CHA_CNT    = 4'h2;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_POST_CHA_LD     = 4'h6;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_POST_CHA_CNT    = 4'h7;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_PREY_LD         = 4'hD;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_PREY_CNT        = 4'h9;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_DATA_CHB_LD     = 4'hB;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_DATA_CHB_CNT    = 4'hA;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_POST_CHB_LD     = 4'hE;
    localparam [3:0] SPDIFTX_FRAMECNT_STATE_POST_CHB_CNT    = 4'hF;
    
    reg [3:0] fcstate;
    
    always @(posedge op_clk)
    begin
        if(reset)
        begin
            fcstate <= SPDIFTX_FRAMECNT_STATE_PREZ_LD;
        end
        else
        begin
            case(fcstate)
                SPDIFTX_FRAMECNT_STATE_PREZ_LD: 
                begin
                    fcstate <= SPDIFTX_FRAMECNT_STATE_PREX_CNT;  
                end
                SPDIFTX_FRAMECNT_STATE_PREX_CNT: 
                begin 
                    if(fcz0) 
                    begin
                        fcstate <= SPDIFTX_FRAMECNT_STATE_DATA_CHA_LD;
                    end
                end        
                SPDIFTX_FRAMECNT_STATE_DATA_CHA_LD:
                begin
                    fcstate <= SPDIFTX_FRAMECNT_STATE_DATA_CHA_CNT;
                end
                SPDIFTX_FRAMECNT_STATE_DATA_CHA_CNT: 
                begin
                    if(fcz0)
                    begin
                        fcstate <= SPDIFTX_FRAMECNT_STATE_POST_CHA_LD;
                    end
                end
                SPDIFTX_FRAMECNT_STATE_POST_CHA_LD: 
                begin
                    fcstate <= SPDIFTX_FRAMECNT_STATE_POST_CHA_CNT;
                end
                SPDIFTX_FRAMECNT_STATE_POST_CHA_CNT:
                begin
                    if(fcz0) 
                    begin
                        fcstate <= SPDIFTX_FRAMECNT_STATE_PREY_LD;
                    end
                end
                SPDIFTX_FRAMECNT_STATE_PREY_LD:
                begin
                    fcstate <= SPDIFTX_FRAMECNT_STATE_PREY_CNT;
                end
                SPDIFTX_FRAMECNT_STATE_PREY_CNT: 
                begin
                    if(fcz0) 
                    begin
                        fcstate <= SPDIFTX_FRAMECNT_STATE_DATA_CHB_LD;
                    end
                end    
                SPDIFTX_FRAMECNT_STATE_DATA_CHB_LD:
                begin
                    fcstate <= SPDIFTX_FRAMECNT_STATE_DATA_CHB_CNT;
                end
                SPDIFTX_FRAMECNT_STATE_DATA_CHB_CNT: 
                begin
                    if(fcz0)
                    begin
                        fcstate <= SPDIFTX_FRAMECNT_STATE_POST_CHB_LD;
                    end
                end
                SPDIFTX_FRAMECNT_STATE_POST_CHB_LD: 
                begin
                    fcstate <= SPDIFTX_FRAMECNT_STATE_POST_CHB_CNT;
                end
                SPDIFTX_FRAMECNT_STATE_POST_CHB_CNT: 
                begin
                    if(fcz0)
                    begin
                        fcstate <= (fcz1) ? SPDIFTX_FRAMECNT_STATE_PREZ_LD : SPDIFTX_FRAMECNT_STATE_PREX_LD;
                    end
                end
                SPDIFTX_FRAMECNT_STATE_PREX_LD:
                begin
                    fcstate <= SPDIFTX_FRAMECNT_STATE_PREX_CNT;
                end
                default: fcstate <= SPDIFTX_FRAMECNT_STATE_PREZ_LD;
            endcase 
        end
    end

    cy_psoc3_dp8 #(.cy_dpconfig_a(
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:  A0 <- period of Z; A1 <- period of block */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:  Count PRE period (A0=A0-1) */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:  Count audio data interval (A0=A0-1) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:  Load audio data period (A0 <- D0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:  Load PREX period (A0 <- F0) */
        `CS_ALU_OP__DEC, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:  A0 <- period of Y; count number of frames (A1=A1-1)*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:  Load period of POST interval (A0<-F0) */
        `CS_ALU_OP__DEC, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:  Count POST interval (A0=A0-1) */
        8'hFF, 8'h00,  /*CFG9:  */
        8'h07, 8'hFF,  /*CFG11-10:  */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_ENBL, `SC_C0_MASK_DSBL,
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
    })) FrameCounter
    (
        /*  input                   */  .reset(1'b0),
        /*  input                   */  .clk(op_clk),
        /*  input   [02:00]         */  .cs_addr(fcstate[2:0]),
        /*  input                   */  .route_si(1'b0),
        /*  input                   */  .route_ci(1'b0),
        /*  input                   */  .f0_load(1'b0),
        /*  input                   */  .f1_load(1'b0),
        /*  input                   */  .d0_load(1'b0),
        /*  input                   */  .d1_load(1'b0),
        /*  output                  */  .ce0(),
        /*  output                  */  .cl0(),
        /*  output                  */  .z0(fcz0),
        /*  output                  */  .ff0(),
        /*  output                  */  .ce1(),
        /*  output                  */  .ce1_reg(fcce1),
        /*  output                  */  .cl1(),
        /*  output                  */  .z1(fcz1),
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
    *                   Preamble generation
    ***************************************************************************/
    /* Preamble generator is implemented as typical pattern generator. It 
    *  generates three patterns loaded into the datapath's registers:
    *    - Preamble Z pattern [11101000](F0);
    *    - Preamble X pattern [11100010](D0);
    *    - Preamble Y pattern [11100100](D1);
    *
    *  The state machine for the PreambleGen has five states. Three states are
    *  used to load corresponding pattern to the accumulators and two states to
    *  shift out the pattern to output. Two shift states are required to implement
    *  shifting for A0 and A1 registers.
    *   
    * The datapath control signals are mapped to use the FrameCounter state machine */ 
    wire [2:0] PreambleGenCtrl = {fcstate[3:2],fcstate[0]}; 
    
    wire preamble_pattern;

    cy_psoc3_dp8 #(.cy_dpconfig_a(
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:  Load PreambleZ (A0<-F0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP___SL, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:  Shift left for D0 and F0 sources */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___D0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:  Load PreambleX (A0<-D0) */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:  Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:  Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP___SL, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:  Shift left for D1 source */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:  Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___D1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:  Load PreambleY (A1<-D1) */
        8'hFF, 8'h00,  /*CFG9:  */
        8'hFF, 8'hFF,  /*CFG11-10:  */
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
    }
    )) PreambleGen(
        /*  input                   */  .reset(1'b0),
        /*  input                   */  .clk(op_clk),
        /*  input   [02:00]         */  .cs_addr(PreambleGenCtrl),
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
        /*  output                  */  .so(),
        /*  output                  */  .so_reg(preamble_pattern),        
        /*  output                  */  .f0_bus_stat(),
        /*  output                  */  .f0_blk_stat(),
        /*  output                  */  .f1_bus_stat(),
        /*  output                  */  .f1_blk_stat()
    );
        
    /***************************************************************************
    *                   Audio data transmitting implementation 
    ****************************************************************************
    *  Each frame consists of 64 bits. Since the component implementation uses 
    *  2X clocking it is required using of doubled period to count frame intervals.
    *  To serialize an audio data a single datapath with a simple state machine 
    *  are used. The datapath operates like a shift register. The state machine 
    *  for the datapath control has only four states.
    */
    
    localparam SPDIFTX_TX_STATE_HOLD  = 2'd0;
    localparam SPDIFTX_TX_STATE_LOAD  = 2'd1;
    localparam SPDIFTX_TX_STATE_SHIFT = 2'd2;
    localparam SPDIFTX_TX_STATE_ZERO  = 2'd3;
    
    reg [1:0] tx_state;
   
    wire [6:0] count;
    wire tc;
    wire data_clock = count[0];  /* Use bit 0 of count7 to divide input clock by 2 */
    
    cy_psoc3_count7 #(.cy_period(7'h7F)) BitCounter
    (
        /*  input             */  .clock(op_clk),
        /*  input             */  .reset(reset),
        /*  input             */  .load(1'b0),
        /*  input             */  .enable(1'b1),
        /*  output  [06:00]   */  .count(count),
        /*  output            */  .tc(tc)
    );
    
    /***************************************************************************
    *                    TX FIFOs Underfow Processing
    ****************************************************************************
    * While transmission is enabled if the transmit FIFO becomes empty and data 
    * is not available for transmission (Transmit underflow) the component will 
    * force the constant transmission of 0's. In addition to transmitting 0's 
    * tx state machine is stopped from fetching any further data.
    */
    reg tx_fifo_empty; /* Determine what particular FIFO status should be used */                       
    always @(posedge op_clk) 
    begin
        tx_fifo_empty <= (InterleavedFifo) ? tx_fifo0_empty : 
                         ~fcstate[3] & tx_fifo0_empty | fcstate[3] & tx_fifo1_empty;       
    end                     
    assign tx_underflow = ((tx_state == SPDIFTX_TX_STATE_LOAD) & tx_fifo_empty);
    
    /* Capture the underflow signal and clear it when the transmit is disabled */
    always @(posedge op_clk)
    begin
        tx_underflow_sticky <= (tx_underflow_sticky | tx_underflow) & ctrl[SPDIF_TX_CTRL_TX_ENABLE];
    end
    
    /* Enabling of the audio data output in the S/PDIF bit stream. Transmission 
    *  begins at the next X or Z frame. So set the enable signal at the 
    *  counter period value. 
    *  Transmission stops at the next rising edge of the clock if any of the 
    *  following events occur: 
    *    - transmit is disabled from software via control register; 
    *    - transmit underflow occurs;
    */    
    reg txenable;
    always @(posedge op_clk)
    begin    
        txenable <= (txenable | tc)         /* start transmit at next X or Z frame */
                   & ctrl[SPDIF_TX_CTRL_TX_ENABLE] /* stop from S/W */
                   & ~tx_underflow_sticky;         /* stop if tx underflow occurs */
    end      
    
    /* Audio data transmission state machine */
    always @(posedge op_clk)
    begin
        if(txenable)
        begin
            if(data_clock == 1'b1)
            begin
                case(count[5:1])  
                    5'h1C: tx_state <= (DataBits > 16) ? SPDIFTX_TX_STATE_LOAD : SPDIFTX_TX_STATE_SHIFT; 
                    5'h14: tx_state <= (DataBits >  8) ? SPDIFTX_TX_STATE_LOAD : SPDIFTX_TX_STATE_SHIFT;
                    5'h0C: tx_state <= SPDIFTX_TX_STATE_LOAD;
                default: tx_state <= SPDIFTX_TX_STATE_SHIFT;
                endcase
            end
            else
            begin
                tx_state <= SPDIFTX_TX_STATE_HOLD;
            end    
        end
        else 
        begin
            tx_state <= SPDIFTX_TX_STATE_ZERO;
        end
    end    
                
    /* Depending on mode of operation (the data is interleaved or not) the audio
    *  data datapath requires four or eight states. In case of data is interleaved 
    *  both channels are loaded from F0 to A0 and shifted to the output. In case
    *  of separated channels audio data are loaded from F0 for ChA and from F1 for
    *  ChB. The most significant bit of frame counter state machine is used to 
    *  distinguish what channel is transmitted.
    */  
    wire [2:0] tx_cfg = (InterleavedFifo) ? {1'b0, tx_state} : {fcstate[3], tx_state};
    wire audio_data;    /* Serial sound data before BMC encoding */
    
    cy_psoc3_dp8 #(.cy_dpconfig_a(
    {
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:  Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:  Load CHA (CHB) if interleaved. Else CHA */
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP___SR, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:  Shift out LSB bit of A0 */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:  Clear A0 */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:  Idle */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:  Load CHB */
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP___SR, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:  Shift out LSB of A1 */
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:  Clear A1 */
        8'hFF, 8'h00,  /*CFG9:  */
        8'hFF, 8'hFF,  /*CFG11-10:  */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12:  */
        `SC_A0_SRC_ACC, `SC_SHIFT_SR, 1'h0,
        1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:  */
        10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*CFG17-16:  */
    }
    )) AudioTx(
            /*  input                   */  .reset(1'b0),
            /*  input                   */  .clk(op_clk),
            /*  input   [02:00]         */  .cs_addr(tx_cfg),
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
            /*  output                  */  .so(),
            /*  output                  */  .so_reg(audio_data),            
            /*  output                  */  .f0_bus_stat(tx_fifo0_not_full), /* FIFO0 not full */
            /*  output                  */  .f0_blk_stat(tx_fifo0_empty),
            /*  output                  */  .f1_bus_stat(tx_fifo1_not_full), /* FIFO1 not full */
            /*  output                  */  .f1_blk_stat(tx_fifo1_empty)
    );
    
    /* Sub-frame periods handling is implemented on the same count7. Each 
    *  period defines what exacly data are currently transmitted.
    */
    localparam [2:0] SPDIFTX_HOLD       = 3'd0;
    localparam [2:0] SPDIFTX_PREAMBLE   = 3'd1;
    localparam [2:0] SPDIFTX_TXDATA     = 3'd3;
    localparam [2:0] SPDIFTX_VALIDITY   = 3'd2;
    localparam [2:0] SPDIFTX_USERDATA   = 3'd6;
    localparam [2:0] SPDIFTX_STATUSDATA = 3'd4;
    localparam [2:0] SPDIFTX_PARITY     = 3'd5;
    
    reg [2:0] subframe_state;
    always @(posedge op_clk)
    begin
        if(reset)
        begin
            subframe_state <= SPDIFTX_HOLD;
        end
        else
        begin
            case(subframe_state)
                SPDIFTX_HOLD: 
                begin
                    if(count[5:1] == 5'h1F)
                        subframe_state <= SPDIFTX_PREAMBLE;
                end
                SPDIFTX_PREAMBLE: 
                begin
                    if(count[5:1] == 5'h1B)
                        subframe_state <= SPDIFTX_TXDATA;
                end
                SPDIFTX_TXDATA:
                begin
                    if(count[5:1] == 5'h03)
                        subframe_state <= SPDIFTX_VALIDITY;
                end
                SPDIFTX_VALIDITY:
                begin
                    if(count[5:1] == 5'h02)
                        subframe_state <= SPDIFTX_USERDATA;  
                end
                SPDIFTX_USERDATA:
                begin
                    if(count[5:1] == 5'h01)
                        subframe_state <= SPDIFTX_STATUSDATA;  
                end
                SPDIFTX_STATUSDATA:
                begin
                    if(count[5:1] == 5'h00)
                        subframe_state <= SPDIFTX_PARITY;  
                end    
                SPDIFTX_PARITY:
                begin
                    if(count[5:1] == 5'h1F)
                        subframe_state <= SPDIFTX_PREAMBLE;  
                end                
            default: subframe_state <= SPDIFTX_HOLD;
            endcase
        end
    end
    
    /***************************************************************************
    *                       Channel Status Generation
    ****************************************************************************
    * Each of the two audio channels has its own status data with a block 
    * structure that repeats every 192 samples, so there exists two channel 
    * status FIFOs. The channel status is byte wide data, least significant byte
    * first. During one sub-frame only one bit of status data is transmitted, so
    * one byte is consumed from these FIFOs every 8 samples. 
    *
    * Channel status state machines has only 3 states (hold, load and shift) 
    * which are replicated for both channels. The data are loaded and shifted on 
    * preamble period loading intervals, defined by frame counter state machine.
    * To distinguish what channels status data are transmitted bit 3 of frame 
    * counter state machine register is used. This bit is 0 for Z and X preambles
    * and 1 for Y preamble. This caused the dependencies between channel status
    * state machine and frame counter state encoding.
    */
    localparam SPDIFTX_STATUSGEN_STATE_RESET = 2'd0;    
    localparam SPDIFTX_STATUSGEN_STATE_HOLD  = 2'd1;
    localparam SPDIFTX_STATUSGEN_STATE_LOAD  = 2'd2;
    localparam SPDIFTX_STATUSGEN_STATE_SHIFT = 2'd3;

    reg [1:0] chstatus_state;

    /***************************************************************************
    *                    Channel Status FIFOs Underfow Processing
    ****************************************************************************
    * While generation of the S/PDIF output is enabled if the status FIFO becomes
    * empty and data is not available for transmission (Status underflow) the 
    * component will send 0's for the channel status and audio data with the 
    * correct generation of X, Y, Z framing and correct parity. In addition to 
    * transmitting 0's tx and status state machines are stopped from fetching any 
    * further data.
    */
    reg cst_fifo_empty; /* Determine what particular FIFO status should be used */
    always @(posedge op_clk) 
    begin
        cst_fifo_empty <= (~fcstate[3] & cst_fifo0_empty) | (fcstate[3] & cst_fifo1_empty);       
    end      
    assign cst_underflow = ((chstatus_state == SPDIFTX_STATUSGEN_STATE_LOAD) & cst_fifo_empty);
    
    /* Capture the underflow signal and clear it when the component is stopped */
    always @(posedge op_clk)
    begin
        if(reset)
        begin
            cst_underflow_sticky <= 1'b0;
        end
        else
        begin
            cst_underflow_sticky <= (cst_underflow_sticky | cst_underflow);
        end
    end    
    
    /* Channel status transmission state machine */ 
    always @(posedge op_clk) 
    begin
        if(reset | cst_underflow_sticky)
        begin
            chstatus_state <= SPDIFTX_STATUSGEN_STATE_RESET;
        end
        else
        begin
            if((fcstate == SPDIFTX_FRAMECNT_STATE_PREZ_LD) | (fcstate == SPDIFTX_FRAMECNT_STATE_PREY_LD) |
            (fcstate == SPDIFTX_FRAMECNT_STATE_PREX_LD))
            begin
                if(fcce1)
                begin
                    chstatus_state <= SPDIFTX_STATUSGEN_STATE_LOAD;
                end
                else
                begin
                    chstatus_state <= SPDIFTX_STATUSGEN_STATE_SHIFT;
                end
            end
            else
            begin
                chstatus_state <= SPDIFTX_STATUSGEN_STATE_HOLD;
            end
        end
    end

    wire [2:0] statusgen_cfg = {fcstate[3], chstatus_state};
    wire chstatus;
    
    cy_psoc3_dp8 #(.cy_dpconfig_a(
    {
        `CS_ALU_OP__XOR, `CS_SRCA_A0, `CS_SRCB_A0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC__ALU, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM0:  Reset*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM1:  Hold ChA*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC___F0, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM2:  Load ChA status byte from F0*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP___SR, `CS_A0_SRC__ALU, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM3:  Shift out ChA status bit*/
        `CS_ALU_OP_PASS, `CS_SRCA_A0, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM4:  Idle*/
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC_NONE,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM5:  Hold ChB*/
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP_PASS, `CS_A0_SRC_NONE, `CS_A1_SRC___F1,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM6:  Load ChB status byte from F1*/
        `CS_ALU_OP_PASS, `CS_SRCA_A1, `CS_SRCB_D0,
        `CS_SHFT_OP___SR, `CS_A0_SRC_NONE, `CS_A1_SRC__ALU,
        `CS_FEEDBACK_DSBL, `CS_CI_SEL_CFGA, `CS_SI_SEL_CFGA,
        `CS_CMP_SEL_CFGA, /*CFGRAM7:  Shift out ChB status bit*/
        8'hFF, 8'h00,  /*CFG9:  */
        8'hFF, 8'hFF,  /*CFG11-10:  */
        `SC_CMPB_A1_D1, `SC_CMPA_A1_D1, `SC_CI_B_ARITH,
        `SC_CI_A_ARITH, `SC_C1_MASK_DSBL, `SC_C0_MASK_DSBL,
        `SC_A_MASK_DSBL, `SC_DEF_SI_0, `SC_SI_B_DEFSI,
        `SC_SI_A_DEFSI, /*CFG13-12:  */
        `SC_A0_SRC_ACC, `SC_SHIFT_SR, 1'h0,
        1'h0, `SC_FIFO1_BUS, `SC_FIFO0_BUS,
        `SC_MSB_DSBL, `SC_MSB_BIT0, `SC_MSB_NOCHN,
        `SC_FB_NOCHN, `SC_CMP1_NOCHN,
        `SC_CMP0_NOCHN, /*CFG15-14:  */
        10'h00, `SC_FIFO_CLK__DP,`SC_FIFO_CAP_AX,
        `SC_FIFO_LEVEL,`SC_FIFO_ASYNC,`SC_EXTCRC_DSBL,
        `SC_WRK16CAT_DSBL /*CFG17-16:  */
    }
    )) ChStatusGen(
        /*  input                   */  .reset(1'b0),
        /*  input                   */  .clk(op_clk),
        /*  input   [02:00]         */  .cs_addr(statusgen_cfg),
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
        /*  output                  */  .so(),
        /*  output                  */  .so_reg(chstatus),
        /*  output                  */  .f0_bus_stat(cst_fifo0_not_full),
        /*  output                  */  .f0_blk_stat(cst_fifo0_empty),
        /*  output                  */  .f1_bus_stat(cst_fifo1_not_full),
        /*  output                  */  .f1_blk_stat(cst_fifo1_empty)
    );
    
    /***************************************************************************
    *                   Post data transition implementation
    ***************************************************************************/
    /* Post data consist of 4 called validity, user, channel status and parity.
    *  The state machine in this case only defines the data that are sent to 
    *  the output for each bit. It has a fixed progression through the states
    *  and starts when post data interval begins.
    *
    *  The validity bit indicates the audio sample is fit for conversion to 
    *  analog. This bit is active low.
    *  The user bit is not defined by standard. Some receivers ignore it, so it 
    *  just is defined as zero.
    *  Parity - even parity bit (bit 0-3 are not included). 
    */ 
   
    localparam SPDIF_TX_USER_BIT     = 1'b0;
    localparam SPDIF_TX_VALIDITY_BIT = 1'b0;
    
    reg bmc_in; /* Data input for BMC encoding */
    
    /* Parity is calulated using data input of BMC encoder. Bit 0-3 should not 
    *  be included, so the parity flag is reset on preamble period.  
    */     
    reg parity;
    
    always @(posedge op_clk)
    begin
        if(subframe_state == SPDIFTX_PREAMBLE)
        begin 
            parity <= 1'b0;
        end
        else
        begin
            if(data_clock)
                parity <= parity ^ bmc_in;
        end
    end

    /***************************************************************************
    *                           Biphase-Marc Encoding 
    ****************************************************************************
    * The signal in the S/PDIF stream switches polarity at every data bit 
    * boundary. The logical level at the start of a bit is always inverted to 
    * the level at the end of the previous bit. The level at the end of a bit is
    * equal (a 0 transmitted) or inverted (a 1 transmitted) to the start of that
    * bit. In other words to transmit a '1' in this format, there is a transition
    * in the middle of the data bit boundary. If there is no transition in the 
    * middle, the data is considered a '0'.
    * The frequency of the clock if twice the bitrate. 
    *    
    * Since preambles are generated as predefined patterns they are passed to 
    *  output without bmc encoding.
    */
    reg bmc_pass;
    reg bmc;    /* BMC encoding output */
    
    always @(posedge op_clk)
    begin
        if(reset)
        begin
            bmc_pass <= 1'b0;
        end
        else
        begin
            bmc_pass <= (subframe_state == SPDIFTX_PREAMBLE);
        end
    end   
    
    /* Determine the data that are encoded to BMC format */ 
    always @(posedge op_clk)
    begin
        if(reset)
        begin
            bmc_in <= 1'b0;
        end
        else
        begin
            case(subframe_state)
                SPDIFTX_PREAMBLE:   bmc_in <= preamble_pattern;
                SPDIFTX_TXDATA:     bmc_in <= audio_data;
                SPDIFTX_VALIDITY:   bmc_in <= SPDIF_TX_VALIDITY_BIT;
                SPDIFTX_USERDATA:   bmc_in <= SPDIF_TX_USER_BIT;
                SPDIFTX_STATUSDATA: bmc_in <= chstatus;
                SPDIFTX_PARITY:     bmc_in <= parity;
            default: bmc_in <= 1'b0;
            endcase
        end   
    end
    
    always @(posedge op_clk)
    begin
        if(subframe_state == SPDIFTX_HOLD)
        begin
            bmc <= 1'b0;
        end
        else
        begin
            if(bmc_pass)
            begin
                bmc <= bmc_in;
            end
            else
            begin
                if((data_clock == 1'b1) | (bmc_in == 1'b1))
                begin
                    bmc <= ~bmc;
                end    
                else
                begin
                    bmc <= bmc;
                end
            end
        end
    end
    
    assign spdif = bmc;
    
endmodule

`endif /* bSPDIF_Tx_v1_10_V_ALREADY_INCLUDED */



